using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;

namespace LINGYUN.Abp.ExceptionHandling
{
    public abstract class AbpExceptionSubscriberBase : ExceptionSubscriber
    {
        protected IServiceScopeFactory ServiceScopeFactory { get; }
        protected AbpExceptionHandlingOptions Options { get; }

        public IAbpLazyServiceProvider ServiceProvider { get; set; }

        protected ILoggerFactory LoggerFactory => ServiceProvider.LazyGetService<ILoggerFactory>();

        protected ILogger Logger => _lazyLogger.Value;
        private Lazy<ILogger> _lazyLogger => new Lazy<ILogger>(() => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance, true);


        protected AbpExceptionSubscriberBase(
            IServiceScopeFactory serviceScopeFactory,
            IOptions<AbpExceptionHandlingOptions> options)
        {
            Options = options.Value;
            ServiceScopeFactory = serviceScopeFactory;
        }

        public override async Task HandleAsync(ExceptionNotificationContext context)
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
