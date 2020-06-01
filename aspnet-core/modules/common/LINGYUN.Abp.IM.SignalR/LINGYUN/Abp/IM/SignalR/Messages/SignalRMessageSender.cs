using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.IM.SignalR.Hubs;
using LINGYUN.Abp.RealTime.Client;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.IM.SignalR.Messages
{
    public class SignalRMessageSender : IMessageSender, ITransientDependency
    {
        public ILogger<SignalRMessageSender> Logger { protected get; set; }

        private readonly IOnlineClientManager _onlineClientManager;

        private readonly IHubContext<AbpMessageHub> _hubContext;

        private readonly IMessageStore _messageStore;

        public SignalRMessageSender(
           IOnlineClientManager onlineClientManager,
           IHubContext<AbpMessageHub> hubContext,
           IMessageStore messageStore)
        {
            _hubContext = hubContext;
            _messageStore = messageStore;
            _onlineClientManager = onlineClientManager;

            Logger = NullLogger<SignalRMessageSender>.Instance;
        }
        /// <summary>
        /// 服务端调用发送消息方法
        /// </summary>
        /// <param name="chatMessage"></param>
        /// <returns></returns>
        public async Task SendMessageAsync(ChatMessage chatMessage)
        {
            // 持久化
            await _messageStore.StoreMessageAsync(chatMessage);

            if (!chatMessage.GroupName.IsNullOrWhiteSpace())
            {
                try
                {
                    var signalRClient = _hubContext.Clients.Group(chatMessage.GroupName);
                    if (signalRClient == null)
                    {
                        Logger.LogDebug("Can not get group " + chatMessage.GroupName + " from SignalR hub!");
                        return;
                    }

                    await signalRClient.SendAsync("getChatMessage", chatMessage);
                }
                catch (Exception ex)
                {
                    Logger.LogWarning("Could not send message to group: {0}", chatMessage.GroupName);
                    Logger.LogWarning("Send group message error: {0}", ex.Message);
                }
            }
            else
            {
                var onlineClientContext = new OnlineClientContext(chatMessage.TenantId, chatMessage.ToUserId);
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

                        await signalRClient.SendAsync("getChatMessage", chatMessage);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWarning("Could not send message to user: {0}", chatMessage.ToUserId);
                        Logger.LogWarning("Send to user message error: {0}", ex.Message);
                    }
                }
            }
        }
    }
}
