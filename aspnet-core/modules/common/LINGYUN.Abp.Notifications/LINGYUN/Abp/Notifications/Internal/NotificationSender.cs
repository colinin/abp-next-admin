using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.Notifications
{
    /// <summary>
    /// 默认实现通过分布式事件发送通知
    /// 可替换实现来发送实时通知
    /// </summary>
    public class NotificationSender : INotificationSender, ITransientDependency
    {
        /// <summary>
        /// Reference to <see cref="ILogger<NotificationSender>"/>.
        /// </summary>
        public ILogger<NotificationSender> Logger { get; set; }
        /// <summary>
        /// Reference to <see cref="IDistributedEventBus"/>.
        /// </summary>
        public IDistributedEventBus DistributedEventBus { get; }

        protected AbpNotificationOptions Options { get; }
        public NotificationSender(
           IDistributedEventBus distributedEventBus,
           IOptions<AbpNotificationOptions> options)
        {
            Options = options.Value;
            DistributedEventBus = distributedEventBus;
            Logger = NullLogger<NotificationSender>.Instance;
        }

        public async Task SendNofiterAsync(
            string name, 
            NotificationData data, 
            UserIdentifier user = null,
            Guid? tenantId = null, 
            NotificationSeverity severity = NotificationSeverity.Info)
        {
            if (user == null)
            {
                await PublishNofiterAsync(name, data, null, tenantId, severity);
                
            }
            else
            {
                await PublishNofiterAsync(name, data, new List<UserIdentifier> { user }, tenantId, severity);
            }
        }

        public async Task SendNofitersAsync(
            string name, 
            NotificationData data,
            IEnumerable<UserIdentifier> users = null,
            Guid? tenantId = null, 
            NotificationSeverity severity = NotificationSeverity.Info)
        {
            await PublishNofiterAsync(name, data, users, tenantId, severity);
        }

        protected async Task PublishNofiterAsync(
            string name, 
            NotificationData data,
            IEnumerable<UserIdentifier> users = null,
            Guid? tenantId = null,
            NotificationSeverity severity = NotificationSeverity.Info)
        {
            await DistributedEventBus
                .PublishAsync(
                    new NotificationEventData
                    {
                        Application = Options.Application,
                        TenantId = tenantId,
                        Users = users?.ToList(),
                        Name = name,
                        Data = data,
                        CreationTime = DateTime.Now,
                        Severity = severity
                    });
        }
    }
}
