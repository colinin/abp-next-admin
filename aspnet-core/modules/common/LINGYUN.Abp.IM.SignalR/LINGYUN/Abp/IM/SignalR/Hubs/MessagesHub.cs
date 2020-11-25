using LINGYUN.Abp.IM.Contract;
using LINGYUN.Abp.IM.Group;
using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.RealTime.Client;
using LINGYUN.Abp.RealTime.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace LINGYUN.Abp.IM.SignalR.Hubs
{
    [Authorize]
    public class MessagesHub : OnlineClientHubBase
    {
        protected AbpIMSignalROptions Options { get; }
        protected IFriendStore FriendStore { get; }
        protected IMessageStore MessageStore { get; }
        protected IUserGroupStore UserGroupStore { get; }

        public MessagesHub(
            IFriendStore friendStore,
            IMessageStore messageStore,
            IUserGroupStore userGroupStore,
            IOptions<AbpIMSignalROptions> options)
        {
            FriendStore = friendStore;
            MessageStore = messageStore;
            UserGroupStore = userGroupStore;
            Options = options.Value;
        }

        protected override async Task OnClientConnectedAsync(IOnlineClient client)
        {
            await base.OnClientConnectedAsync(client);
            // 加入通讯组
            var userGroups = await UserGroupStore.GetUserGroupsAsync(client.TenantId, client.UserId.Value);
            foreach (var group in userGroups)
            {
                await Groups.AddToGroupAsync(client.ConnectionId, group.Name);
                var groupClient = Clients.Group(group.Name);
                if (groupClient != null)
                {
                    // 发送用户上线通知
                    await groupClient.SendAsync(Options.UserOnlineMethod, client.TenantId, client.UserId.Value);
                }
            }

            // 发送好友上线通知
            var userFriends = await FriendStore.GetListAsync(client.TenantId, client.UserId.Value);
            if (userFriends.Count > 0)
            {
                var friendClientIds = userFriends.Select(friend => friend.FriendId.ToString()).ToImmutableArray();
                var userClients = Clients.Users(friendClientIds);
                if (userClients != null)
                {
                    await userClients.SendAsync(Options.UserOnlineMethod, client.TenantId, client.UserId.Value);
                }
            }
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
            await MessageStore.StoreMessageAsync(chatMessage, cancellationToken: Context.ConnectionAborted);

            if (!chatMessage.GroupId.IsNullOrWhiteSpace())
            {
                await SendMessageToGroupAsync(chatMessage);
            }
            else
            {
                await SendMessageToUserAsync(chatMessage);
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

            await signalRClient.SendAsync(Options.GetChatMessageMethod, chatMessage, cancellationToken: Context.ConnectionAborted);
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
                    await signalRClient.SendAsync(Options.GetChatMessageMethod, chatMessage, cancellationToken: Context.ConnectionAborted);
                }
                catch (Exception ex)
                {
                    // 发送异常记录就行了,因为消息已经持久化
                    Logger.LogWarning("Could not send message to user: {0}", chatMessage.ToUserId);
                    Logger.LogWarning("Send to user message error: {0}", ex.Message);
                }
            }
        }
    }
}
