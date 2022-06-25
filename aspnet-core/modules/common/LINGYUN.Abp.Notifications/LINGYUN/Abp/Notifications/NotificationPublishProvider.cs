using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.Notifications
{
    public abstract class NotificationPublishProvider : INotificationPublishProvider, ITransientDependency
    {
        public abstract string Name { get; }

        public IAbpLazyServiceProvider ServiceProvider { protected get; set; }

        public ILoggerFactory LoggerFactory => ServiceProvider.LazyGetRequiredService<ILoggerFactory>();

        protected ILogger Logger => _lazyLogger.Value;
        private Lazy<ILogger> _lazyLogger => new Lazy<ILogger>(() => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance, true);

        public ICancellationTokenProvider CancellationTokenProvider => ServiceProvider.LazyGetService<ICancellationTokenProvider>(NullCancellationTokenProvider.Instance);

        public async Task PublishAsync(NotificationInfo notification, IEnumerable<UserIdentifier> identifiers)
        {
            await PublishAsync(notification, identifiers, CancellationTokenProvider.Token);
        }
        /// <summary>
        /// 重写实现通知发布
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="identifiers"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected abstract Task PublishAsync(NotificationInfo notification, IEnumerable<UserIdentifier> identifiers, CancellationToken cancellationToken = default);
    }
}
