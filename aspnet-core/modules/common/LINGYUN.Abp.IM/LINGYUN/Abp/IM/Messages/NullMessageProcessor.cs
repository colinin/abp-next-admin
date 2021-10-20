using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.IM.Messages
{
    [Dependency(TryRegister = true)]
    public class NullMessageProcessor : IMessageProcessor, ISingletonDependency
    {
        public Task ReadAsync(ChatMessage message)
        {
            return Task.CompletedTask;
        }

        public Task ReCallAsync(ChatMessage message)
        {
            return Task.CompletedTask;
        }
    }
}
