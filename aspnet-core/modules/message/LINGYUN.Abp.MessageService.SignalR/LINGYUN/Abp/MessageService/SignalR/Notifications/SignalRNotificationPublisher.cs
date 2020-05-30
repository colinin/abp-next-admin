using LINGYUN.Abp.IM;
using LINGYUN.Abp.MessageService.Notifications;
using LINGYUN.Abp.MessageService.SignalR.Hubs;
using LINGYUN.Abp.Notifications;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.MessageService.SignalR.Notifications
{
    public class SignalRNotificationPublisher : INotificationPublisher, ITransientDependency
    {
        public ILogger<SignalRNotificationPublisher> Logger { protected get; set; }

        private readonly IOnlineClientManager _onlineClientManager;

        private readonly IHubContext<AbpMessageHub> _hubContext;

        public SignalRNotificationPublisher(
            IOnlineClientManager onlineClientManager,
            IHubContext<AbpMessageHub> hubContext)
        {
            _hubContext = hubContext;
            _onlineClientManager = onlineClientManager;

            Logger = NullLogger<SignalRNotificationPublisher>.Instance;
        }

        public async Task PublishAsync(NotificationData data, IEnumerable<Guid> userIds, Guid? tenantId)
        {
            try
            {
                foreach(var userId in userIds)
                {
                    var onlineClientContext = new OnlineClientContext(userId, tenantId);
                    var onlineClients = _onlineClientManager.GetAllByContext(onlineClientContext);
                    foreach (var onlineClient in onlineClients)
                    {
                        var signalRClient = _hubContext.Clients.Client(onlineClient.ConnectionId);
                        if (signalRClient == null)
                        {
                            Logger.LogDebug("Can not get user " + onlineClientContext.UserId + " with connectionId " + onlineClient.ConnectionId + " from SignalR hub!");
                            continue;
                        }
                        await signalRClient.SendAsync("getNotification", data);
                    }
                }
            }
            catch(Exception ex)
            {
                //Logger.LogWarning("Could not send notification to user: " + userId);
                Logger.LogWarning(ex.ToString(), ex);
            }
        }
    }
}
