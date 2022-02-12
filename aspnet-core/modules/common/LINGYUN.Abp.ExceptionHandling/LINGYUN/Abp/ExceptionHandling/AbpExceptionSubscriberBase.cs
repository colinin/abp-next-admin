using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.ExceptionHandling;

namespace LINGYUN.Abp.ExceptionHandling
{
    public abstract class AbpExceptionSubscriberBase : ExceptionSubscriber
    {
        protected IServiceScopeFactory ServiceScopeFactory { get; }
        protected AbpExceptionHandlingOptions Options { get; }

        public IServiceProvider ServiceProvider { get; set; }
        protected readonly object ServiceProviderLock = new object();

        protected TService LazyGetRequiredService<TService>(ref TService reference)
            => LazyGetRequiredService(typeof(TService), ref reference);

        protected TRef LazyGetRequiredService<TRef>(Type serviceType, ref TRef reference)
        {
            if (reference == null)
            {
                lock (ServiceProviderLock)
                {
                    if (reference == null)
                    {
                        reference = (TRef)ServiceProvider.GetRequiredService(serviceType);
                    }
                }
            }

            return reference;
        }

        protected ILoggerFactory LoggerFactory => LazyGetRequiredService(ref _loggerFactory);
        private ILoggerFactory _loggerFactory;

        protected ILogger Logger => LazyLogger.Value;
        private Lazy<ILogger> LazyLogger => new Lazy<ILogger>(() => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance, true);


        protected AbpExceptionSubscriberBase(
            IServiceScopeFactory serviceScopeFactory,
            IOptions<AbpExceptionHandlingOptions> options)
        {
            Options = options.Value;
            ServiceScopeFactory = serviceScopeFactory;
        }

        public async override Task HandleAsync(ExceptionNotificationContext context)
        {
            if (context.Handled &&
                Options.HasNotifierError(context.Exception))
            {
                using (var scope = ServiceScopeFactory.CreateScope())
                {
                    await SendErrorNotifierAsync(
                        new ExceptionSendNotifierContext(scope.ServiceProvider, context.Exception, context.LogLevel));
                }
            }
        }

        protected abstract Task SendErrorNotifierAsync(ExceptionSendNotifierContext context);
    }
}
