using LINGYUN.Abp.IM;
using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.RealTime;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.MessageService.EventBus.Distributed
{
    public class ChatMessageEventHandler : IDistributedEventHandler<RealTimeEto<ChatMessage>>, ITransientDependency
    {
        /// <summary>
        /// Reference to <see cref="ILogger<DefaultNotificationDispatcher>"/>.
        /// </summary>
        public ILogger<ChatMessageEventHandler> Logger { get; set; }
        /// <summary>
        /// Reference to <see cref="AbpIMOptions"/>.
        /// </summary>
        protected AbpIMOptions Options { get; }

        protected IMessageStore MessageStore { get; }
        protected IMessageSenderProviderManager MessageSenderProviderManager { get; }

        public ChatMessageEventHandler(
            IOptions<AbpIMOptions> options,
            IMessageStore messageStore,
            IMessageSenderProviderManager messageSenderProviderManager)
        {
            Options = options.Value;
            MessageStore = messageStore;
            MessageSenderProviderManager = messageSenderProviderManager;
            
            Logger = NullLogger<ChatMessageEventHandler>.Instance;
        }

        public virtual async Task HandleEventAsync(RealTimeEto<ChatMessage> eventData)
        {
            Logger.LogDebug($"Persistent chat message.");

            await MessageStore.StoreMessageAsync(eventData.Data);

            // 发送消息
            foreach (var provider in MessageSenderProviderManager.Providers)
            {
                Logger.LogDebug($"Sending message with provider {provider.Name}");
                await provider.SendMessageAsync(eventData.Data);
            }
        }
    }
}
