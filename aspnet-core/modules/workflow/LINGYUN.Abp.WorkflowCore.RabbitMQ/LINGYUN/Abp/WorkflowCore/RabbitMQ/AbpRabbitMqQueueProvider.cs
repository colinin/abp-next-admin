using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.RabbitMQ;
using Volo.Abp.Threading;
using WorkflowCore.Interface;

namespace LINGYUN.Abp.WorkflowCore.RabbitMQ
{
    [Dependency(ReplaceServices = true)]
    public class AbpRabbitMqQueueProvider : IQueueAdapterProvider, ISingletonDependency
    {
        protected bool IsDiposed { get; private set; }
        protected SemaphoreSlim SyncObj = new SemaphoreSlim(1, 1);

        protected IChannelAccessor ChannelAccessor { get; private set; }

        protected IChannelPool ChannelPool { get; }
        protected IQueueNameNormalizer QueueNameNormalizer { get; }
        protected AbpRabbitMQWorkflowCoreOptions RabbitMQWorkflowCoreOptions { get; }

        public ILogger<AbpRabbitMqQueueProvider> Logger { get; set; }

        public bool IsDequeueBlocking => false;

        public AbpRabbitMqQueueProvider(
            IChannelPool channelPool,
            IQueueNameNormalizer queueNameNormalizer,
            IOptions<AbpRabbitMQWorkflowCoreOptions> options)
        {
            ChannelPool = channelPool;
            QueueNameNormalizer = queueNameNormalizer;
            RabbitMQWorkflowCoreOptions = options.Value;

            Logger = NullLogger<AbpRabbitMqQueueProvider>.Instance;
        }

        public async Task<string> DequeueWork(QueueType queue, CancellationToken cancellationToken)
        {
            // TODO: 存在已知的问题,在多租户情况下, 从队列获取的工作流标识将无法查询到工作流实例
            CheckDisposed();

            using (await SyncObj.LockAsync(cancellationToken))
            {
                await EnsureInitializedAsync();

                ChannelAccessor.Channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                var msg = ChannelAccessor.Channel.BasicGet(QueueNameNormalizer.NormalizeKey(queue), false);
                if (msg != null)
                {
                    var data = Encoding.UTF8.GetString(msg.Body.ToArray());
                    ChannelAccessor.Channel.BasicAck(msg.DeliveryTag, false);
                    return data;
                }
                return null;
            }
        }

        public async Task QueueWork(string id, QueueType queue)
        {
            await QueueWorkAsync(id, queue);
        }

        protected virtual async Task QueueWorkAsync(string id, QueueType queue)
        {
            CheckDisposed();

            using (await SyncObj.LockAsync())
            {
                await EnsureInitializedAsync();

                var body = Encoding.UTF8.GetBytes(id);
                
                ChannelAccessor.Channel.BasicPublish(
                    exchange: "",
                    routingKey: QueueNameNormalizer.NormalizeKey(queue),
                    basicProperties: null,
                    body: body
                );
            }
        }

        public async Task Start()
        {
            CheckDisposed();

            using (await SyncObj.LockAsync())
            {
                await EnsureInitializedAsync();
            }
        }

        public Task Stop()
        {
            Dispose();

            return Task.CompletedTask;
        }


        public void Dispose()
        {
            if (IsDiposed)
            {
                return;
            }

            IsDiposed = true;

            ChannelAccessor?.Dispose();
        }


        protected virtual Task EnsureInitializedAsync()
        {
            if (ChannelAccessor != null)
            {
                return Task.CompletedTask;
            }

            ChannelAccessor = ChannelPool.Acquire(
                RabbitMQWorkflowCoreOptions.DefaultChannelName,
                RabbitMQWorkflowCoreOptions.DefaultConnectionName
            );

            CreateDeclareWorkflowQueue(QueueType.Event);
            CreateDeclareWorkflowQueue(QueueType.Workflow);
            CreateDeclareWorkflowQueue(QueueType.Index);

            return Task.CompletedTask;
        }

        protected virtual void CreateDeclareWorkflowQueue(QueueType queue)
        {
            var queueName = QueueNameNormalizer.NormalizeKey(queue);
            var configuration = new WorkflowQueueConfiguration(
                queueName: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            configuration.Declare(ChannelAccessor.Channel);
        }

        protected void CheckDisposed()
        {
            if (IsDiposed)
            {
                throw new AbpException("This object is disposed!");
            }
        }
    }
}
