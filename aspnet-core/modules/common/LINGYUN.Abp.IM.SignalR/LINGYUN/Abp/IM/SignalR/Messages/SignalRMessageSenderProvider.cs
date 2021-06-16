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
           IServiceProvider serviceProvider,
           IOptions<AbpIMSignalROptions> options)
            : base(serviceProvider)
        {
            _options = options.Value;
            _hubContext = hubContext;
            _onlineClientManager = onlineClientManager;
        }

        protected override async Task SendMessageToGroupAsync(ChatMessage chatMessage)
        {
            var signalRClient = _hubContext.Clients.Group(chatMessage.GroupId);
            if (signalRClient == null)
            {
                Logger.LogDebug("Can not get group " + chatMessage.GroupId + " from SignalR hub!");
                return;
            }

            await signalRClient.SendAsync(_options.GetChatMessageMethod, chatMessage);
        }

        protected override async Task SendMessageToUserAsync(ChatMessage chatMessage)
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
            }
        }
    }
}
