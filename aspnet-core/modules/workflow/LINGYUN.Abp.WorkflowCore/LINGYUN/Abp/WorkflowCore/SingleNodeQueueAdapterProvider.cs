using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.WorkflowCore
{
    [Dependency(TryRegister = true)]
    public class SingleNodeQueueAdapterProvider : IQueueAdapterProvider, ISingletonDependency
    {
        private readonly Dictionary<QueueType, BlockingCollection<string>> _queues = new Dictionary<QueueType, BlockingCollection<string>>
        {
            [QueueType.Workflow] = new BlockingCollection<string>(),
            [QueueType.Event] = new BlockingCollection<string>(),
            [QueueType.Index] = new BlockingCollection<string>()
        };

        public bool IsDequeueBlocking => true;

        public Task QueueWork(string id, QueueType queue)
        {
            _queues[queue].Add(id);
            return Task.CompletedTask;
        }

        public Task<string> DequeueWork(QueueType queue, CancellationToken cancellationToken)
        {
            if (_queues[queue].TryTake(out string id, 100, cancellationToken))
                return Task.FromResult(id);

            return Task.FromResult<string>(null);
        }

        public Task Start()
        {
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}
