using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Uow;
using WorkflowCore.Interface;

namespace LINGYUN.Abp.WorkflowCore
{
    /// <summary>
    /// 当触发新的工作流时，如果持久层在事务中会导致默认队列获取工作流实例失败
    /// 建立一个事务适配队列在工作单元结束时再将消息入队
    /// </summary>
    public class AbpUnitOfWorkQueueProvider : IQueueProvider
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IQueueAdapterProvider _queueProvider;

        public AbpUnitOfWorkQueueProvider(
            IQueueAdapterProvider queueProvider,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _queueProvider = queueProvider;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public bool IsDequeueBlocking => _queueProvider.IsDequeueBlocking;

        public virtual async Task<string> DequeueWork(QueueType queue, CancellationToken cancellationToken)
        {
            if (_unitOfWorkManager.Current != null && !_unitOfWorkManager.Current.IsCompleted)
            {
                return null;
            }

            return await _queueProvider.DequeueWork(queue, cancellationToken);
        }

        public void Dispose()
        {
            _queueProvider.Dispose();
        }

        public async Task QueueWork(string id, QueueType queue)
        {
            if (_unitOfWorkManager.Current != null)
            {
                _unitOfWorkManager.Current.OnCompleted(async () =>
                {
                    await _queueProvider.QueueWork(id, queue);
                });
            }
            else
            {
                await _queueProvider.QueueWork(id, queue);
            }
        }

        public async Task Start()
        {
            await _queueProvider.Start();
        }

        public async Task Stop()
        {
            await _queueProvider.Stop();
        }
    }
}
