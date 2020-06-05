using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.RealTime.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.IM.SignalR.Hubs
{
    [Authorize]
    public class MessageHub : OnlineClientHubBase
    {
        private readonly IMessageStore _messageStore;

        public MessageHub(
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

            try
            {
                if (!chatMessage.GroupId.IsNullOrWhiteSpace())
                {
                    await SendMessageToGroupAsync(chatMessage);
                }
                else
                {
                    await SendMessageToUserAsync(chatMessage);
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Could not send message, group: {0}, formUser: {1}, toUser: {2}",
                    chatMessage.GroupId, chatMessage.FormUserName,
                    chatMessage.ToUserId.HasValue ? chatMessage.ToUserId.ToString() : "None");
                Logger.LogWarning("Send group message error: {0}", ex.Message);
            }
        }

        protected virtual async Task SendMessageToGroupAsync(ChatMessage chatMessage)
        {
            var signalRClient = Clients.Group(chatMessage.GroupId);
            if (signalRClient == null)
            {
                Logger.LogDebug("Can not get group " + chatMessage.GroupId + " from SignalR hub!");
                return;
            }

            await signalRClient.SendAsync("getChatMessage", chatMessage);
        }

        protected virtual async Task SendMessageToUserAsync(ChatMessage chatMessage)
        {
            var onlineClientContext = new OnlineClientContext(chatMessage.TenantId, chatMessage.ToUserId.GetValueOrDefault());
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
