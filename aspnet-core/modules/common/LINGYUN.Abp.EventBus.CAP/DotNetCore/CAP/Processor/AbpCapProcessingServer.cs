using DotNetCore.CAP.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetCore.CAP.Processor
{
    /// <summary>
    /// CapProcessingServer
    /// </summary>
    public class AbpCapProcessingServer : IProcessingServer
    {
        private readonly CancellationTokenSource _cts;
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IServiceProvider _provider;

        private Task _compositeTask;
        private ProcessingContext _context;
        private bool _disposed;
        /// <summary>
        /// CapProcessingServer
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="provider"></param>
        public AbpCapProcessingServer(
            ILogger<AbpCapProcessingServer> logger,
            ILoggerFactory loggerFactory,
            IServiceProvider provider)
        {
            _logger = logger;
            _loggerFactory = loggerFactory;
            _provider = provider;
            _cts = new CancellationTokenSource();
        }
        /// <summary>
        /// Start
        /// </summary>
        public void Start(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting the processing server.");
            stoppingToken.Register(() =>
            {
                _cts.Cancel();
            });

            _context = new ProcessingContext(_provider, _cts.Token);

            var processorTasks = GetProcessors()
                .Select(InfiniteRetry)
                .Select(p => p.ProcessAsync(_context));
            _compositeTask = Task.WhenAll(processorTasks);
        }
        /// <summary>
        /// Pulse
        /// </summary>
        public void Pulse()
        {
            _logger.LogTrace("Pulsing the processor.");
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            try
            {
                _disposed = true;

                _logger.LogInformation("Shutting down the processing server...");
                _cts.Cancel();

                _compositeTask?.Wait((int)TimeSpan.FromSeconds(10).TotalMilliseconds);
            }
            catch (AggregateException ex)
            {
                var innerEx = ex.InnerExceptions[0];
                if (!(innerEx is OperationCanceledException))
                {
                    _logger.LogWarning(innerEx, $"Expected an OperationCanceledException, but found '{innerEx.Message}'.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "An exception was occured when disposing.");
            }
            finally
            {
                _logger.LogInformation("### CAP shutdown!");
            }
        }

        private IProcessor InfiniteRetry(IProcessor inner)
        {
            return new InfiniteRetryProcessor(inner, _loggerFactory);
        }

        private IProcessor[] GetProcessors()
        {
            var returnedProcessors = new List<IProcessor>
            {
                _provider.GetRequiredService<TransportCheckProcessor>(),
                _provider.GetRequiredService<MessageNeedToRetryProcessor>(),
            };

            return returnedProcessors.ToArray();
        }
    }
}
