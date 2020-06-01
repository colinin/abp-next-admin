using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.RealTime.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.SignalR;

namespace LINGYUN.Abp.IM.SignalR.Hubs
{
    [Authorize]
    [HubRoute("messages")]
    public class AbpMessageHub : OnlineClientHubBase
    {
        private readonly IMessageStore _messageStore;

        public AbpMessageHub(
            IMessageStore messageStore)
        {
            _messageStore = messageStore;
        }
        /// <summary>
        /// 客户端调用发送消息方法
        /// </summary>
        /// <param name="chatMessage"></param>
        /// <returns></returns>
        [HubMethodName("SendMessage")]
        public virtual async Task SendMessageAsync(ChatMessage chatMessage)
        {
            // 持久化
            await _messageStore.StoreMessageAsync(chatMessage);

            if (!chatMessage.GroupName.IsNullOrWhiteSpace())
            {
                try
                {
                    var signalRClient = Clients.Group(chatMessage.GroupName);
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
                var onlineClients = OnlineClientManager.GetAllByContext(onlineClientContext);
                foreach (var onlineClient in onlineClients)
                {
                    try
                    {
                        var signalRClient = Clients.Client(onlineClient.ConnectionId);
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
