using LINGYUN.Abp.Notifications.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications.SignalR
{
    public class SignalRNotificationPublishProvider : NotificationPublishProvider
    {
        public const string ProviderName = "SignalR";
        public override string Name => ProviderName;

        private readonly IHubContext<NotificationsHub> _hubContext;

        private readonly AbpNotificationsSignalROptions _options;
        public SignalRNotificationPublishProvider(
           IHubContext<NotificationsHub> hubContext,
           IOptions<AbpNotificationsSignalROptions> options,
           IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _options = options.Value;
            _hubContext = hubContext;
        }

        protected override async Task PublishAsync(NotificationInfo notification, IEnumerable<UserIdentifier> identifiers, CancellationToken cancellationToken = default)
        {
            if (identifiers?.Count() == 0)
            {
                var groupName = notification.TenantId?.ToString() ?? "Global";

                var singalRGroup = _hubContext.Clients.Group(groupName);
                // 租户通知群发
                Logger.LogDebug($"Found a singalr group, begin senging notifications");
                await singalRGroup.SendAsync(_options.MethodName, notification, cancellationToken);
            }
            else
            {
                try
                {
                    var onlineClients = _hubContext.Clients.Users(identifiers.Select(x => x.UserId.ToString()));
                    Logger.LogDebug($"Found a singalr client, begin senging notifications");
                    await onlineClients.SendAsync(_options.MethodName, notification, cancellationToken);
                }
                catch (Exception ex)
                {
                    Logger.LogWarning("Could not send notifications to all users");
                    Logger.LogWarning("Send to user notifications error: {0}", ex.Message);
                }
            }
        }
    }
}
