using DotNetCore.CAP;
using DotNetCore.CAP.Internal;
using DotNetCore.CAP.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.EventBus.CAP;

public class AbpCAPBootstrapper : IBootstrapper
{
    private readonly ILogger<IBootstrapper> _logger;
    private readonly IServiceProvider _serviceProvider;

    private CancellationTokenSource _cts;
    private bool _disposed;
    private IEnumerable<IProcessingServer> _processors = default!;

    public bool IsStarted => !_cts?.IsCancellationRequested ?? false;

    public AbpCAPBootstrapper(IServiceProvider serviceProvider, ILogger<IBootstrapper> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task BootstrapAsync(CancellationToken cancellationToken = default)
    {
        if (_cts != null)
        {
            _logger.LogInformation("### CAP background task is already started!");

            return;
        }

        _logger.LogDebug("### CAP background task is starting.");

        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        CheckRequirement();

        _processors = _serviceProvider.GetServices<IProcessingServer>();

        try
        {
            await _serviceProvider.GetRequiredService<IStorageInitializer>().InitializeAsync(_cts.Token).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            if (e is InvalidOperationException) throw;
            _logger.LogError(e, "Initializing the storage structure failed!");
        }

        _cts.Token.Register(() =>
        {
            _logger.LogDebug("### CAP background task is stopping.");


            foreach (var item in _processors)
                try
                {
                    item.Dispose();
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogWarning(ex, $"Expected an OperationCanceledException, but found '{ex.Message}'.");
                }
        });

        await BootstrapCoreAsync().ConfigureAwait(false);

        _disposed = false;
        _logger.LogInformation("### CAP started!");
    }

    protected virtual async Task BootstrapCoreAsync()
    {
        foreach (var item in _processors)
        {
            try
            {
                _cts!.Token.ThrowIfCancellationRequested();

                await item.Start(_cts!.Token);
            }
            catch (OperationCanceledException)
            {
                // ignore
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Starting the processors throw an exception.");
            }
        }
    }

    public virtual void Dispose()
    {
        if (_disposed) return;

        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
        _disposed = true;
    }

    public virtual async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await BootstrapAsync(stoppingToken).ConfigureAwait(false);
    }

    public virtual Task StopAsync(CancellationToken cancellationToken)
    {
        _cts?.Cancel();

        return Task.CompletedTask;
    }

    private void CheckRequirement()
    {
        var marker = _serviceProvider.GetService<CapMarkerService>();
        if (marker == null)
            throw new InvalidOperationException(
                "AddCap() must be added on the service collection.   eg: services.AddCap(...)");

        var messageQueueMarker = _serviceProvider.GetService<CapMessageQueueMakerService>();
        if (messageQueueMarker == null)
            throw new InvalidOperationException(
                "You must be config transport provider for CAP!" + Environment.NewLine +
                "==================================================================================" +
                Environment.NewLine +
                "========   eg: services.AddCap( options => { options.UseRabbitMQ(...) }); ========" +
                Environment.NewLine +
                "==================================================================================");

        var databaseMarker = _serviceProvider.GetService<CapStorageMarkerService>();
        if (databaseMarker == null)
            throw new InvalidOperationException(
                "You must be config storage provider for CAP!" + Environment.NewLine +
                "===================================================================================" +
                Environment.NewLine +
                "========   eg: services.AddCap( options => { options.UseSqlServer(...) }); ========" +
                Environment.NewLine +
                "===================================================================================");
    }

    public ValueTask DisposeAsync()
    {
        Dispose();

        return ValueTask.CompletedTask;
    }
}
