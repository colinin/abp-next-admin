using LINGYUN.Abp.Notifications.SignalR.Hubs;
using LINGYUN.Abp.RealTime.Client;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Notifications.SignalR
{
    public class SignalRNotificationPublishProvider : NotificationPublishProvider
    {
        public const string ProviderName = "SignalR";
        public override string Name => ProviderName;

        private readonly IOnlineClientManager _onlineClientManager;

        private readonly IHubContext<NotificationsHub> _hubContext;

        public SignalRNotificationPublishProvider(
           IOnlineClientManager onlineClientManager,
           IHubContext<NotificationsHub> hubContext,
           IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _hubContext = hubContext;
            _onlineClientManager = onlineClientManager;
        }

        public override async Task PublishAsync(NotificationInfo notification, IEnumerable<UserIdentifier> identifiers)
        {
            // 返回标准数据给前端
            notification.Data = NotificationData.ToStandardData(notification.Data);

            foreach (var identifier in identifiers)
            {
                var onlineClientContext = new OnlineClientContext(notification.TenantId, identifier.UserId);
                var onlineClients = _onlineClientManager.GetAllByContext(onlineClientContext);
                foreach (var onlineClient in onlineClients)
                {
                    try
                    {
                        var signalRClient = _hubContext.Clients.Client(onlineClient.ConnectionId);
                        if (signalRClient == null)
                        {
                            Logger.LogDebug("Can not get user " + onlineClientContext.UserId + " with connectionId " + onlineClient.ConnectionId + " from SignalR hub!");
                            continue;
                        }

                        await signalRClient.SendAsync("getNotification", notification);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWarning("Could not send notifications to user: {0}", identifier.UserId);
                        Logger.LogWarning("Send to user notifications error: {0}", ex.Message);
                    }
                }
            }
        }
    }
}
