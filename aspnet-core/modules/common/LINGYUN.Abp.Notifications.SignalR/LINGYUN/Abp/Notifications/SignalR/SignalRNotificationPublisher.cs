using LINGYUN.Abp.Notifications.SignalR.Hubs;
using LINGYUN.Abp.RealTime.Client;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Notifications.SignalR
{
    public class SignalRNotificationPublisher : INotificationPublisher, ISingletonDependency
    {
        public ILogger<SignalRNotificationPublisher> Logger { protected get; set; }

        private readonly IOnlineClientManager _onlineClientManager;

        private readonly IHubContext<AbpNotificationsHub> _hubContext;

        public SignalRNotificationPublisher(
           IOnlineClientManager onlineClientManager,
           IHubContext<AbpNotificationsHub> hubContext)
        {
            _hubContext = hubContext;
            _onlineClientManager = onlineClientManager;

            Logger = NullLogger<SignalRNotificationPublisher>.Instance;
        }

        public async Task PublishAsync(NotificationInfo notification, IEnumerable<Guid> userIds)
        {
            foreach(var userId in userIds)
            {
                var onlineClientContext = new OnlineClientContext(notification.TenantId, userId);
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
                        Logger.LogWarning("Could not send notifications to user: {0}", userId);
                        Logger.LogWarning("Send to user notifications error: {0}", ex.Message);
                    }
                }
            }
        }
    }
}
