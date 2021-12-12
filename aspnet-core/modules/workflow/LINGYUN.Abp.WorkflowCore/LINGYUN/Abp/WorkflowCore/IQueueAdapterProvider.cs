using System;
using System.Threading;
using System.Threading.Tasks;
using WorkflowCore.Interface;

namespace LINGYUN.Abp.WorkflowCore
{
    public interface IQueueAdapterProvider : IDisposable
    {
        bool IsDequeueBlocking { get; }

        Task QueueWork(string id, QueueType queue);

        Task<string> DequeueWork(QueueType queue, CancellationToken cancellationToken);

        Task Start();

        Task Stop();
    }
}
