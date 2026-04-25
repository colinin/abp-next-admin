using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AuditLogging;
public abstract class AuditLoggingQueue<TLog>
{
    private readonly ILogger<AuditLoggingQueue<TLog>> _logger;
    private readonly Channel<TLog> _channel;
    private readonly string _logName;
    private readonly int _batchSize;
    private readonly int _maxConcurrency;

    private volatile Task _consumerTask;
    private readonly SemaphoreSlim _flushSemaphore;
    private readonly CancellationTokenSource _cts = new CancellationTokenSource();
    protected AuditLoggingQueue(
        string logName,
        int maxQueueSize,
        int batchSize,
        ILogger<AuditLoggingQueue<TLog>> logger)
    {
        _logName = logName;
        _batchSize = batchSize;
        _logger = logger;

        var channelOptions = new BoundedChannelOptions(maxQueueSize)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleWriter = false,
            SingleReader = true,
        };
        _channel = Channel.CreateBounded<TLog>(channelOptions);

        var calculatedConcurrency = (int)Math.Ceiling(Math.Max(1, maxQueueSize) / (double)Math.Max(1, batchSize));
        var defaultConcurrency = Environment.ProcessorCount * 2;
        _maxConcurrency = Math.Min(calculatedConcurrency, defaultConcurrency);
        _flushSemaphore = new SemaphoreSlim(_maxConcurrency, _maxConcurrency);
    }

    public virtual Task EnqueueAsync(TLog log, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!_channel.Writer.TryWrite(log))
            {
                _logger.LogWarning("{logName} channel is full; {logName} is recorded in the log.", _logName, _logName);
                _logger.LogInformation(log!.ToString());
            }

            return Task.CompletedTask;
        }
        catch (ChannelClosedException)
        {
            _logger.LogWarning("{logName} channel is closed; dropping it.", _logName);
        }
        catch (OperationCanceledException)
        {
            _logger.LogDebug("EnqueueAsync was canceled while waiting to write to {logName} channel.", _logName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected exception while enqueuing {logName}.", _logName);
        }

        return Task.CompletedTask;
    }

    public virtual Task StartAsync(CancellationToken cancellationToken)
    {
        _consumerTask = Task.Factory.StartNew(
            () => ConsumeBatchesAsync(),
            CancellationToken.None,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default)
            .Unwrap();

        return Task.CompletedTask;
    }

    public async virtual Task StopAsync(CancellationToken cancellationToken)
    {
        _channel.Writer.TryComplete();

        if (_consumerTask != null)
        {
            try
            {
                await Task.WhenAny(_consumerTask, Task.Delay(TimeSpan.FromSeconds(5), cancellationToken));
                _cts.Cancel();
                _logger.LogInformation("Stopped consumer loop: {logName}.", _logName);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("StopAsync was canceled while waiting for consumer loop: {logName} to finish.", _logName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while stopping consumer loop: {logName}.", _logName);
            }
            finally
            {
                _cts.Dispose();
            }
        }
    }

    private async Task ConsumeBatchesAsync()
    {
        var reader = _channel.Reader;
        var batchSize = Math.Max(1, _batchSize);
        var batchSendTaskList = new List<Task>(_maxConcurrency * 2);

        _logger.LogInformation("Starting {logName} consumer loop. BatchSize={batchSize}", _logName, batchSize);

        try
        {
            while (await reader.WaitToReadAsync(_cts.Token))
            {
                var buffer = new List<TLog>(batchSize);

                while (buffer.Count < batchSize && reader.TryRead(out var item))
                {
                    buffer.Add(item);
                }

                if (buffer.Count > 0)
                {
                    batchSendTaskList.Add(SendBatchAsync(buffer, _cts.Token));
                    if (batchSendTaskList.Count >= _maxConcurrency)
                    {
                        try
                        {
                            var completedTask = await Task.WhenAny(batchSendTaskList);
                            batchSendTaskList.Remove(completedTask);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Background send {logName} task failed.", _logName);
                        }
                    }
                }
            }

            await Task.WhenAll(batchSendTaskList);
        }
        catch (OperationCanceledException)
        {
            // ignore
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected exception in {logName} consumer loop.", _logName);
        }
    }

    private async Task SendBatchAsync(List<TLog> batch, CancellationToken cancellationToken)
    {
        if (batch == null || batch.Count == 0)
        {
            return;
        }

        await _flushSemaphore.WaitAsync(cancellationToken);

        try
        {
            if (batch.Count == 1)
            {
                await WriteAsync(batch[0], cancellationToken);
            }
            else
            {
                await BulkWriteAsync(batch, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogDebug("SendBatchAsync canceled while writing {logName}.", _logName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to write {logName} batch to Elasticsearch. Items={count}", _logName, batch.Count);
        }
        finally
        {
            _flushSemaphore.Release();
        }
    }

    protected abstract Task WriteAsync(TLog log, CancellationToken cancellationToken = default);
    protected abstract Task BulkWriteAsync(IEnumerable<TLog> logs, CancellationToken cancellationToken = default);
}
