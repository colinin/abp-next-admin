﻿using LINGYUN.Abp.IdGenerator;
using LINGYUN.Abp.IM.Contract;
using LINGYUN.Abp.IM.Groups;
using LINGYUN.Abp.IM.Localization;
using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.RealTime.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
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

        protected AbpExceptionHandlingOptions ExceptionHandlingOptions => LazyServiceProvider.LazyGetRequiredService<IOptions<AbpExceptionHandlingOptions>>().Value;


        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            try
            {
                await SendUserOnlineStateAsync();
            }
            catch (OperationCanceledException)
            {
                // Ignore
                return;
            }
            catch (Exception ex)
            {
                Logger.LogWarning("An error occurred in the OnConnected method:{message}", ex.Message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);

            try
            {
                await SendUserOnlineStateAsync(false);
            }
            catch (OperationCanceledException)
            {
                // Ignore
                return;
            }
            catch (Exception ex)
            {
                Logger.LogWarning("An error occurred in the OnDisconnected method:{message}", ex.Message);
            }
        }

        protected async virtual Task SendUserOnlineStateAsync(bool isOnlined = true)
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
        public async virtual Task<string> SendMessageAsync(ChatMessage chatMessage)
        {
            return await SendMessageAsync(Options.GetChatMessageMethod, chatMessage, true);
        }

        [HubMethodName("recall")]
        public async virtual Task ReCallAsync(ChatMessage chatMessage)
        {
            try
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
            catch (Exception ex)
            {
                if (ex is IBusinessException)
                {
                    var errorInfo = ErrorInfoConverter.Convert(ex, options =>
                    {
                        options.SendExceptionsDetailsToClients = ExceptionHandlingOptions.SendExceptionsDetailsToClients;
                        options.SendStackTraceToClients = ExceptionHandlingOptions.SendStackTraceToClients;
                    });

                    await SendMessageAsync(
                        Options.ReCallChatMessageMethod,
                        ChatMessage.System(
                            chatMessage.ToUserId.Value,
                            chatMessage.FormUserId,
                            errorInfo.Message,
                            Clock,
                            MessageType.Notifier,
                            chatMessage.TenantId)
                        .SetProperty(nameof(ChatMessage.MessageId).ToPascalCase(), chatMessage.MessageId),
                        callbackException: false);
                }
            }
        }

        [HubMethodName("read")]
        public async virtual Task ReadAsync(ChatMessage chatMessage)
        {
            try
            {
                await Processor?.ReadAsync(chatMessage);
            }
            catch (OperationCanceledException)
            {
                // Ignore
                return;
            }
            catch (Exception ex)
            {
                Logger.LogWarning("An error occurred in the Read method:{message}", ex.Message);
            }
        }

        protected async virtual Task<string> SendMessageAsync(string methodName, ChatMessage chatMessage, bool callbackException = false)
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

                return chatMessage.MessageId;
            }
            catch (Exception ex)
            {
                if (callbackException && ex is IBusinessException)
                {
                    var errorInfo = ErrorInfoConverter.Convert(ex, options =>
                    {
                        options.SendExceptionsDetailsToClients = ExceptionHandlingOptions.SendExceptionsDetailsToClients;
                        options.SendStackTraceToClients = ExceptionHandlingOptions.SendStackTraceToClients;
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
                                MessageType.Notifier,
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
                                MessageType.Notifier,
                                chatMessage.TenantId));
                    }
                }
            }

            return "";
        }

        protected async virtual Task SendMessageToGroupAsync(string methodName, ChatMessage chatMessage)
        {
            var signalRClient = Clients.Group(chatMessage.GroupId);
            await signalRClient.SendAsync(methodName, chatMessage);
        }

        protected async virtual Task SendMessageToUserAsync(string methodName, ChatMessage chatMessage)
        {
            var onlineClients = Clients.User(chatMessage.ToUserId.GetValueOrDefault().ToString());
            await onlineClients.SendAsync(methodName, chatMessage);
        }
    }
}
