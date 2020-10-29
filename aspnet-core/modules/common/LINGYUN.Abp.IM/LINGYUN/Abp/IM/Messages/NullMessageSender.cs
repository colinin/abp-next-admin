using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.IM.Messages
{
    public class NullMessageSender : MessageSenderBase, ITransientDependency
    {
        public NullMessageSender(IMessageStore store, ILogger<NullMessageSender> logger) 
            : base(store, logger)
        {
        }

        protected override Task SendMessageToGroupAsync(ChatMessage chatMessage)
        {
            Logger.LogWarning("No IMessageSender Interface implementation!");
            return Task.CompletedTask;
        }

        protected override Task SendMessageToUserAsync(ChatMessage chatMessage)
        {
            Logger.LogWarning("No IMessageSender Interface implementation!");
            return Task.CompletedTask;
        }
    }
}
