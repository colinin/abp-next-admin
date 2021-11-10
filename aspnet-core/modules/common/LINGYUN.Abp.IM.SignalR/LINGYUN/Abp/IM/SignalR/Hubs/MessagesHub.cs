using LINGYUN.Abp.IM.Contract;
using LINGYUN.Abp.IM.Groups;
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
        protected IMessageProcessor Processor => LazyServiceProvider.LazyGetRequiredService<IMessageProcessor>();

        protected IUserOnlineChanger OnlineChanger => LazyServiceProvider.LazyGetService<IUserOnlineChanger>();

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

            // 用户上线
            await OnlineChanger?.ChangeAsync(client.TenantId, client.UserId.Value, UserOnlineState.Online);

            await SendUserOnlineStateAsync(client);
        }

        protected override async Task OnClientDisconnectedAsync(IOnlineClient client)
        {
            await base.OnClientDisconnectedAsync(client);

            // 用户下线
            await OnlineChanger?.ChangeAsync(client.TenantId, client.UserId.Value, UserOnlineState.Offline);

            await SendUserOnlineStateAsync(client, false);
        }

        protected virtual async Task SendUserOnlineStateAsync(IOnlineClient client, bool isOnlined = true)
        {
            var methodName = isOnlined ? Options.UserOnlineMethod : Options.UserOfflineMethod;

            var userGroups = await UserGroupStore.GetUserGroupsAsync(client.TenantId, client.UserId.Value);
            foreach (var group in userGroups)
            {
                if (isOnlined)
                {
                    // 应使用群组标识
                    await Groups.AddToGroupAsync(client.ConnectionId, group.Id);
                }
                var groupClient = Clients.Group(group.Id);
                if (groupClient != null)
                {
                    // 发送用户下线通知
                    await groupClient.SendAsync(methodName, client.TenantId, client.UserId.Value);
                }
            }

            var userFriends = await FriendStore.GetListAsync(client.TenantId, client.UserId.Value);
            if (userFriends.Count > 0)
            {
                var friendClientIds = userFriends.Select(friend => friend.FriendId.ToString()).ToImmutableArray();
                var userClients = Clients.Users(friendClientIds);
                if (userClients != null)
                {
                    await userClients.SendAsync(methodName, client.TenantId, client.UserId.Value);
                }
            }
        }
        /// <summary>
        /// 客户端调用发送消息方法
        /// </summary>
        /// <param name="chatMessage"></param>
        /// <returns></returns>
        // [HubMethodName("SendMessage")]
        [HubMethodName("send")]
        public virtual async Task SendAsync(ChatMessage chatMessage)
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

        [HubMethodName("recall")]
        public virtual async Task ReCallAsync(ChatMessage chatMessage)
        {
            await Processor.ReCallAsync(chatMessage);
        }

        [HubMethodName("read")]
        public virtual async Task ReadAsync(ChatMessage chatMessage)
        {
            await Processor.ReadAsync(chatMessage);
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
