using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.IM.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.IM.SignalR.Messages
{
    public class SignalRMessageSenderProvider : MessageSenderProviderBase
    {
        public override string Name => "SignalR";
        private readonly AbpIMSignalROptions _options;

        private readonly IHubContext<MessagesHub> _hubContext;

        public SignalRMessageSenderProvider(
           IHubContext<MessagesHub> hubContext,
           IAbpLazyServiceProvider serviceProvider,
           IOptions<AbpIMSignalROptions> options)
            : base(serviceProvider)
        {
            _options = options.Value;
            _hubContext = hubContext;
        }

        protected override async Task SendMessageToGroupAsync(ChatMessage chatMessage)
        {
            await TrySendMessageToGroupAsync(chatMessage, true);
        }

        protected override async Task SendMessageToUserAsync(ChatMessage chatMessage)
        {
            await TrySendMessageToUserAsync(chatMessage, true);
        }

        protected virtual async Task TrySendMessageToGroupAsync(ChatMessage chatMessage, bool sendingExceptionCallback = true)
        {
            try
            {
                var signalRClient = _hubContext.Clients.Group(chatMessage.GroupId);
                if (signalRClient == null)
                {
                    Logger.LogDebug("Can not get group " + chatMessage.GroupId + " from SignalR hub!");
                    return;
                }

                await signalRClient.SendAsync(_options.GetChatMessageMethod, chatMessage);
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Could not send message to group: {0}", chatMessage.GroupId);
                Logger.LogWarning("Send to group message error: {0}", ex.Message);

                if (sendingExceptionCallback)
                {
                    await TrySendBusinessErrorMessage(ex, chatMessage);
                }
            }
        }

        protected virtual async Task TrySendMessageToUserAsync(ChatMessage chatMessage, bool sendingExceptionCallback = true)
        {
            try
            {
                var onlineClients = _hubContext.Clients.User(chatMessage.ToUserId.Value.ToString());
                if (onlineClients == null)
                {
                    Logger.LogDebug("Can not get user " + chatMessage.ToUserId + " connection from SignalR hub!");
                    return;
                }
                await onlineClients.SendAsync(_options.GetChatMessageMethod, chatMessage);
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Could not send message to user: {0}", chatMessage.ToUserId);
                Logger.LogWarning("Send to user message error: {0}", ex.Message);

                if (sendingExceptionCallback)
                {
                    await TrySendBusinessErrorMessage(ex, chatMessage);
                }
            }
        }

        protected virtual async Task TrySendBusinessErrorMessage(Exception ex, ChatMessage chatMessage)
        {
            if (ex is IBusinessException)
            {
                var clock = ServiceProvider.LazyGetRequiredService<IClock>();
                var errorInfoConverter = ServiceProvider.LazyGetService<IExceptionToErrorInfoConverter>();
                if (errorInfoConverter != null)
                {
                    var errorInfo = errorInfoConverter.Convert(ex, options =>
                    {
                        options.SendExceptionsDetailsToClients = false;
                        options.SendStackTraceToClients = false;
                    });
                    if (!chatMessage.GroupId.IsNullOrWhiteSpace())
                    {
                        await TrySendMessageToGroupAsync(
                            ChatMessage.System(
                                chatMessage.FormUserId,
                                chatMessage.GroupId,
                                errorInfo.Message,
                                clock,
                                chatMessage.MessageType,
                                chatMessage.TenantId)
                            .SetProperty(nameof(ChatMessage.MessageId).ToPascalCase(), chatMessage.MessageId),
                            sendingExceptionCallback: false);
                    }
                    else
                    {
                        await TrySendMessageToUserAsync(
                            ChatMessage.System(
                                chatMessage.FormUserId,
                                chatMessage.ToUserId.Value,
                                errorInfo.Message,
                                clock,
                                chatMessage.MessageType,
                                chatMessage.TenantId)
                            .SetProperty(nameof(ChatMessage.MessageId).ToPascalCase(), chatMessage.MessageId),
                            sendingExceptionCallback: false);
                    }
                }
            }
        }
    }
}
