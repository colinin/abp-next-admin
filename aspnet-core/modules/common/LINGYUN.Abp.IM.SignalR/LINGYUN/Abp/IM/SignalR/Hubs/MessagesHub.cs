using LINGYUN.Abp.IdGenerator;
using LINGYUN.Abp.IM.Contract;
using LINGYUN.Abp.IM.Groups;
using LINGYUN.Abp.IM.Localization;
using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.RealTime;
using LINGYUN.Abp.RealTime.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Data;
using Volo.Abp.Localization;
using Volo.Abp.Users;

namespace LINGYUN.Abp.IM.SignalR.Hubs
{
    [Authorize]
    public class MessagesHub : AbpHub
    {
        protected IMessageProcessor Processor => LazyServiceProvider.LazyGetService<IMessageProcessor>();

        protected IUserOnlineChanger OnlineChanger => LazyServiceProvider.LazyGetService<IUserOnlineChanger>();

        protected IDistributedIdGenerator DistributedIdGenerator => LazyServiceProvider.LazyGetRequiredService<IDistributedIdGenerator>();

        protected IExceptionToErrorInfoConverter ErrorInfoConverter => LazyServiceProvider.LazyGetRequiredService<IExceptionToErrorInfoConverter>();

        protected AbpIMSignalROptions Options => LazyServiceProvider.LazyGetRequiredService<IOptions<AbpIMSignalROptions>>().Value;

        protected IFriendStore FriendStore => LazyServiceProvider.LazyGetRequiredService<IFriendStore>();

        protected IMessageStore MessageStore => LazyServiceProvider.LazyGetRequiredService<IMessageStore>();

        protected IUserGroupStore UserGroupStore => LazyServiceProvider.LazyGetRequiredService<IUserGroupStore>();

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            await SendUserOnlineStateAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);

            await SendUserOnlineStateAsync(false);
        }

        protected virtual async Task SendUserOnlineStateAsync(bool isOnlined = true)
        {
            var methodName = isOnlined ? Options.UserOnlineMethod : Options.UserOfflineMethod;

            var userGroups = await UserGroupStore.GetUserGroupsAsync(CurrentTenant.Id, CurrentUser.GetId());
            foreach (var group in userGroups)
            {
                if (isOnlined)
                {
                    // 应使用群组标识
                    await Groups.AddToGroupAsync(Context.ConnectionId, group.Id);
                }
                var groupClient = Clients.Group(group.Id);
                await groupClient.SendAsync(methodName, CurrentTenant.Id, CurrentUser.GetId());
            }

            var userFriends = await FriendStore.GetListAsync(CurrentTenant.Id, CurrentUser.GetId());
            if (userFriends.Count > 0)
            {
                var friendClientIds = userFriends.Select(friend => friend.FriendId.ToString()).ToImmutableArray();
                var userClients = Clients.Users(friendClientIds);
                await userClients.SendAsync(methodName, CurrentTenant.Id, CurrentUser.GetId());
            }
        }
        /// <summary>
        /// 客户端调用发送消息方法
        /// </summary>
        /// <param name="chatMessage"></param>
        /// <returns></returns>
        [HubMethodName("send")]
        public virtual async Task SendMessageAsync(ChatMessage chatMessage)
        {
            await SendMessageAsync(Options.GetChatMessageMethod, chatMessage, true);
        }

        [HubMethodName("recall")]
        public virtual async Task ReCallAsync(ChatMessage chatMessage)
        {
            await Processor?.ReCallAsync(chatMessage);
            if (!chatMessage.GroupId.IsNullOrWhiteSpace())
            {
                await SendMessageAsync(
                    Options.ReCallChatMessageMethod,
                    ChatMessage.SystemLocalized(
                        chatMessage.FormUserId,
                        chatMessage.GroupId,
                        new LocalizableStringInfo(
                            LocalizationResourceNameAttribute.GetName(typeof(AbpIMResource)),
                            "Messages:RecallMessage",
                            new Dictionary<object, object>
                            {
                                { "User", chatMessage.FormUserName }
                            }),
                        Clock,
                        chatMessage.MessageType,
                        chatMessage.TenantId)
                    .SetProperty(nameof(ChatMessage.MessageId).ToPascalCase(), chatMessage.MessageId),
                    callbackException: false);
            }
            else
            {
                await SendMessageAsync(
                    Options.ReCallChatMessageMethod,
                    ChatMessage.SystemLocalized(
                        chatMessage.ToUserId.Value,
                        chatMessage.FormUserId,
                        new LocalizableStringInfo(
                            LocalizationResourceNameAttribute.GetName(typeof(AbpIMResource)),
                            "Messages:RecallMessage",
                            new Dictionary<object, object>
                            {
                                { "User", chatMessage.FormUserName }
                            }),
                        Clock,
                        chatMessage.MessageType,
                        chatMessage.TenantId)
                    .SetProperty(nameof(ChatMessage.MessageId).ToPascalCase(), chatMessage.MessageId),
                    callbackException: false);
            }
        }

        [HubMethodName("read")]
        public virtual async Task ReadAsync(ChatMessage chatMessage)
        {
            await Processor?.ReadAsync(chatMessage);
        }

        protected virtual async Task SendMessageAsync(string methodName, ChatMessage chatMessage, bool callbackException = false)
        {
            // 持久化
            try
            {
                chatMessage.SetProperty(nameof(ChatMessage.IsAnonymous), chatMessage.IsAnonymous);
                chatMessage.MessageId = DistributedIdGenerator.Create().ToString();
                await MessageStore.StoreMessageAsync(chatMessage);

                if (!chatMessage.GroupId.IsNullOrWhiteSpace())
                {
                    await SendMessageToGroupAsync(methodName, chatMessage);
                }
                else
                {
                    await SendMessageToUserAsync(methodName, chatMessage);
                }
            }
            catch (Exception ex)
            {
                if (callbackException && ex is IBusinessException)
                {
                    var errorInfo = ErrorInfoConverter.Convert(ex, options =>
                    {
                        options.SendExceptionsDetailsToClients = false;
                        options.SendStackTraceToClients = false;
                    });
                    if (!chatMessage.GroupId.IsNullOrWhiteSpace())
                    {
                        await SendMessageToGroupAsync(
                            methodName,
                            ChatMessage.System(
                                chatMessage.FormUserId,
                                chatMessage.GroupId,
                                errorInfo.Message,
                                Clock,
                                chatMessage.MessageType,
                                chatMessage.TenantId));
                    }
                    else
                    {
                        await SendMessageToUserAsync(
                            methodName,
                            ChatMessage.System(
                                chatMessage.ToUserId.Value,
                                chatMessage.FormUserId,
                                errorInfo.Message,
                                Clock,
                                chatMessage.MessageType,
                                chatMessage.TenantId));
                    }
                }
            }
        }

        protected virtual async Task SendMessageToGroupAsync(string methodName, ChatMessage chatMessage)
        {
            var signalRClient = Clients.Group(chatMessage.GroupId);
            await signalRClient.SendAsync(methodName, chatMessage);
        }

        protected virtual async Task SendMessageToUserAsync(string methodName, ChatMessage chatMessage)
        {
            var onlineClients = Clients.User(chatMessage.ToUserId.GetValueOrDefault().ToString());
            await onlineClients.SendAsync(methodName, chatMessage);
        }
    }
}
