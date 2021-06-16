using LINGYUN.Abp.Notifications.SignalR.Hubs;
using LINGYUN.Abp.RealTime.Client;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications.SignalR
{
    public class SignalRNotificationPublishProvider : NotificationPublishProvider
    {
        public const string ProviderName = "SignalR";
        public override string Name => ProviderName;

        private readonly IOnlineClientManager _onlineClientManager;

        private readonly IHubContext<NotificationsHub> _hubContext;

        private readonly AbpNotificationsSignalROptions _options;
        public SignalRNotificationPublishProvider(
           IOnlineClientManager onlineClientManager,
           IHubContext<NotificationsHub> hubContext,
           IOptions<AbpNotificationsSignalROptions> options,
           IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _options = options.Value;
            _hubContext = hubContext;
            _onlineClientManager = onlineClientManager;
        }

        protected override async Task PublishAsync(NotificationInfo notification, IEnumerable<UserIdentifier> identifiers, CancellationToken cancellationToken = default)
        {
            if (identifiers?.Count() == 0)
            {
                var groupName = notification.TenantId?.ToString() ?? "Global";

                var singalRGroup = _hubContext.Clients.Group(groupName);
                if (singalRGroup == null)
                {
                    Logger.LogDebug("Can not get group " + groupName + " from SignalR hub!");
                    return;
                }
                // 租户通知群发
                Logger.LogDebug($"Found a singalr group, begin senging notifications");
                await singalRGroup.SendAsync(_options.MethodName, notification, cancellationToken);
            }
            else
            {
                var onlineClients = _onlineClientManager.GetAllClients(client => identifiers.Any(ids => client.UserId == ids.UserId));
                var onlineClientConnectionIds = onlineClients.Select(client => client.ConnectionId).ToImmutableArray();
                try
                {
                    var signalRClients = _hubContext.Clients.Clients(onlineClientConnectionIds);
                    if (signalRClients == null)
                    {
                        Logger.LogDebug("Can not get users connection from SignalR hub!");
                        return;
                    }
                    Logger.LogDebug($"Found a singalr client, begin senging notifications");
                    await signalRClients.SendAsync(_options.MethodName, notification, cancellationToken);
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
