using LINGYUN.Abp.IdGenerator;
using LINGYUN.Abp.RealTime;
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
        /// <summary>
        /// Reference to <see cref="IDistributedIdGenerator"/>.
        /// </summary>
        protected IDistributedIdGenerator DistributedIdGenerator { get; }

        protected AbpNotificationOptions Options { get; }
        public NotificationSender(
           IDistributedEventBus distributedEventBus,
           IDistributedIdGenerator distributedIdGenerator,
           IOptions<AbpNotificationOptions> options)
        {
            Options = options.Value;
            DistributedEventBus = distributedEventBus;
            DistributedIdGenerator = distributedIdGenerator;
            Logger = NullLogger<NotificationSender>.Instance;
        }

        public async Task<string> SendNofiterAsync(
            string name, 
            NotificationData data, 
            UserIdentifier user = null,
            Guid? tenantId = null, 
            NotificationSeverity severity = NotificationSeverity.Info)
        {
            if (user == null)
            {
                return await PublishNofiterAsync(name, data, null, tenantId, severity);
                
            }
            else
            {
                return await  PublishNofiterAsync(name, data, new List<UserIdentifier> { user }, tenantId, severity);
            }
        }

        public async Task<string> SendNofitersAsync(
            string name, 
            NotificationData data,
            IEnumerable<UserIdentifier> users = null,
            Guid? tenantId = null, 
            NotificationSeverity severity = NotificationSeverity.Info)
        {
            return await PublishNofiterAsync(name, data, users, tenantId, severity);
        }

        protected async Task<string> PublishNofiterAsync(
            string name, 
            NotificationData data,
            IEnumerable<UserIdentifier> users = null,
            Guid? tenantId = null,
            NotificationSeverity severity = NotificationSeverity.Info)
        {
            var eto = new NotificationEto<NotificationData>(data)
            {
                Id = DistributedIdGenerator.Create(),
                TenantId = tenantId,
                Users = users?.ToList(),
                Name = name,
                CreationTime = DateTime.Now,
                Severity = severity
            };

            await DistributedEventBus.PublishAsync(eto);

            return eto.Id.ToString();
        }
    }
}
