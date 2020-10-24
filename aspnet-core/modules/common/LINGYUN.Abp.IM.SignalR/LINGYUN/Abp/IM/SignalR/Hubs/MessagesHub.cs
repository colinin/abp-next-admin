using LINGYUN.Abp.IM.Contract;
using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.RealTime.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.IM.SignalR.Hubs
{
    [Authorize]
    public class MessagesHub : OnlineClientHubBase
    {
        protected IFriendStore FriendStore { get; }
        protected IMessageStore MessageStore { get; }

        public MessagesHub(
            IFriendStore friendStore,
            IMessageStore messageStore)
        {
            FriendStore = friendStore;
            MessageStore = messageStore;
        }

        [HubMethodName("LastContactFriends")]
        public virtual async Task<PagedResultDto<UserFriend>> GetLastContactFriendsAsync(
            Guid? tenantId,
            Guid userId,
            int skipCount = 0,
            int maxResultCount = 10)
        {
            var myFrientCount = await FriendStore.GetCountAsync(tenantId, userId);

            var lastContractFriends = await FriendStore
                .GetLastContactListAsync(tenantId, userId, skipCount, maxResultCount);

            return new PagedResultDto<UserFriend>(myFrientCount, lastContractFriends);
        }

        [HubMethodName("MyFriends")]
        public virtual async Task<PagedResultDto<UserFriend>> GetMyFriendsAsync(
            Guid? tenantId,
            Guid userId,
            string filter = "",
            string sorting = nameof(UserFriend.UserId),
            bool reverse = false,
            int skipCount = 0,
            int maxResultCount = 10)
        {
            var myFrientCount = await FriendStore.GetCountAsync(tenantId, userId);

            var myFriends = await FriendStore
                .GetListAsync(tenantId, userId, filter, sorting, reverse, skipCount, maxResultCount);

            return new PagedResultDto<UserFriend>(myFrientCount, myFriends);
        }

        [HubMethodName("AddFriend")]
        public virtual async Task AddFriendAsync(
            Guid? tenantId,
            Guid userId,
            Guid friendId,
            string remarkName = "")
        {
            await FriendStore.AddMemberAsync(tenantId, userId, friendId, remarkName);
        }

        [HubMethodName("RemoveFriend")]
        public virtual async Task RemoveFriendAsync(
            Guid? tenantId,
            Guid userId,
            Guid friendId,
            string remarkName = "")
        {
            await FriendStore.RemoveMemberAsync(tenantId, userId, friendId);
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
            await MessageStore.StoreMessageAsync(chatMessage);

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

            // 需要捕捉每一个发送任务的异常吗?
            // var onlineClientConnections = onlineClients.Select(c => c.ConnectionId).ToImmutableList();
            // var signalRClient = Clients.Clients(onlineClientConnections);
            // await signalRClient.SendAsync("getChatMessage", chatMessage);

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
