using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.IM.Messages
{
    public class MessageSender : IMessageSender, ITransientDependency
    {
        protected IDistributedEventBus EventBus { get; }
        public MessageSender(IDistributedEventBus eventBus)
        {
            EventBus = eventBus;
        }

        public virtual async Task SendMessageAsync(ChatMessage chatMessage)
        {
            chatMessage.SetProperty(nameof(ChatMessage.IsAnonymous), chatMessage.IsAnonymous);

            // 如果先存储的话,就紧耦合消息处理模块了
            // await Store.StoreMessageAsync(chatMessage);

            await EventBus.PublishAsync(chatMessage);
        }
    }
}
