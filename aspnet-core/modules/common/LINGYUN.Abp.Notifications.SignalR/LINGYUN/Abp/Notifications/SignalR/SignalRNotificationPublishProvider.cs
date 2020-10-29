using LINGYUN.Abp.Notifications.SignalR.Hubs;
using LINGYUN.Abp.RealTime.Client;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
            if (notification.Data.HasTenantNotification(out Guid tenantId))
            {
                // 返回标准数据给前端
                notification.Data = NotificationData.ToStandardData(notification.Data);
                var singalRGroup = _hubContext.Clients.Group(tenantId.ToString());
                if (singalRGroup == null)
                {
                    Logger.LogDebug("Can not get group " + tenantId + " from SignalR hub!");
                    return;
                }
                // 租户通知群发
                Logger.LogDebug($"Found a singalr group, begin senging notifications");
                await singalRGroup.SendAsync("getNotification", notification);
            }
            else
            {
                // 返回标准数据给前端
                notification.Data = NotificationData.ToStandardData(notification.Data);
                foreach (var identifier in identifiers)
                {
                    Logger.LogDebug($"Find online client with user {identifier.UserId} - {identifier.UserName}");
                    var onlineClientContext = new OnlineClientContext(notification.TenantId, identifier.UserId);
                    var onlineClients = _onlineClientManager.GetAllByContext(onlineClientContext);
                    var onlineClientConnectionIds = onlineClients.Select(client => client.ConnectionId).ToImmutableArray();
                    try
                    {
                        var signalRClients = _hubContext.Clients.Clients(onlineClientConnectionIds);
                        if (signalRClients == null)
                        {
                            Logger.LogDebug("Can not get user " + onlineClientContext.UserId + " connection from SignalR hub!");
                            return;
                        }
                        Logger.LogDebug($"Found a singalr client, begin senging notifications");
                        await signalRClients.SendAsync("getNotification", notification);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWarning("Could not send notifications to user: {0}", identifier.UserId);
                        Logger.LogWarning("Send to user notifications error: {0}", ex.Message);
                    }

                    //foreach (var onlineClient in onlineClients)
                    //{
                    //    try
                    //    {
                    //        Logger.LogDebug($"Find online client {onlineClient.UserId} - {onlineClient.ConnectionId}");
                    //        var signalRClient = _hubContext.Clients.Client(onlineClient.ConnectionId);
                    //        if (signalRClient == null)
                    //        {
                    //            Logger.LogDebug("Can not get user " + onlineClientContext.UserId + " with connectionId " + onlineClient.ConnectionId + " from SignalR hub!");
                    //            continue;
                    //        }
                    //        Logger.LogDebug($"Found a singalr client, begin senging notifications");
                    //        await signalRClient.SendAsync("getNotification", notification);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Logger.LogWarning("Could not send notifications to user: {0}", identifier.UserId);
                    //        Logger.LogWarning("Send to user notifications error: {0}", ex.Message);
                    //    }
                    //}
                }
            }
        }
    }
}
