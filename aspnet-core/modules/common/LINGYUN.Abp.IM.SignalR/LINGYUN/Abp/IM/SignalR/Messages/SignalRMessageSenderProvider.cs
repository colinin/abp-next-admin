using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.IM.SignalR.Hubs;
using LINGYUN.Abp.RealTime.Client;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Immutable;
using System.Linq;
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

        private readonly IOnlineClientManager _onlineClientManager;

        private readonly IHubContext<MessagesHub> _hubContext;

        public SignalRMessageSenderProvider(
           IOnlineClientManager onlineClientManager,
           IHubContext<MessagesHub> hubContext,
           IAbpLazyServiceProvider serviceProvider,
           IOptions<AbpIMSignalROptions> options)
            : base(serviceProvider)
        {
            _options = options.Value;
            _hubContext = hubContext;
            _onlineClientManager = onlineClientManager;
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
                var onlineClientContext = new OnlineClientContext(chatMessage.TenantId, chatMessage.ToUserId.Value);
                var onlineClients = _onlineClientManager.GetAllByContext(onlineClientContext);

                var onlineClientConnectionIds = onlineClients.Select(client => client.ConnectionId).ToImmutableArray();
                var signalRClients = _hubContext.Clients.Clients(onlineClientConnectionIds);
                if (signalRClients == null)
                {
                    Logger.LogDebug("Can not get user " + onlineClientContext.UserId + " connection from SignalR hub!");
                    return;
                }
                await signalRClients.SendAsync(_options.GetChatMessageMethod, chatMessage);
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
