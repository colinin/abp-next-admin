using LINGYUN.Abp.IdGenerator;
using LINGYUN.Abp.RealTime;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.IM.Messages
{
    public class MessageSender : IMessageSender, ITransientDependency
    {
        protected IDistributedEventBus EventBus { get; }
        protected IDistributedIdGenerator DistributedIdGenerator { get; }
        public MessageSender(
            IDistributedEventBus eventBus,
            IDistributedIdGenerator distributedIdGenerator)
        {
            EventBus = eventBus;
            DistributedIdGenerator = distributedIdGenerator;
        }

        public virtual async Task<string> SendMessageAsync(ChatMessage chatMessage)
        {
            chatMessage.SetProperty(nameof(ChatMessage.IsAnonymous), chatMessage.IsAnonymous);
            chatMessage.MessageId = DistributedIdGenerator.Create().ToString();
            // 如果先存储的话,就紧耦合消息处理模块了
            // await Store.StoreMessageAsync(chatMessage);
            var eto = new RealTimeEto<ChatMessage>(chatMessage);

            await EventBus.PublishAsync(eto);

            return chatMessage.MessageId;
        }
    }
}
