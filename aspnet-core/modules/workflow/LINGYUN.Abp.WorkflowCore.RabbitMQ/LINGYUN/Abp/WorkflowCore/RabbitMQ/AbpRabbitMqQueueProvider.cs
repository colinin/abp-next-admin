using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.RabbitMQ;
using Volo.Abp.Threading;
using WorkflowCore.Interface;

namespace LINGYUN.Abp.WorkflowCore.RabbitMQ
{
    public class AbpRabbitMqQueueProvider : IQueueProvider
    {
        private string ChannelPrefix = "WorkflowQueue.";
        protected bool IsDiposed { get; private set; }
        protected SemaphoreSlim SyncObj = new SemaphoreSlim(1, 1);

        protected IChannelAccessor ChannelAccessor { get; private set; }

        protected IChannelPool ChannelPool { get; }
        protected IQueueNameNormalizer QueueNameNormalizer { get; }
        protected AbpRabbitMQWorkflowCoreOptions RabbitMQWorkflowCoreOptions { get; }
        protected WorkflowQueueConfiguration QueueConfiguration { get; }

        public bool IsDequeueBlocking => false;

        public AbpRabbitMqQueueProvider(
            IChannelPool channelPool,
            IQueueNameNormalizer queueNameNormalizer,
            IOptions<AbpRabbitMQWorkflowCoreOptions> options)
        {
            ChannelPool = channelPool;
            QueueNameNormalizer = queueNameNormalizer;
            RabbitMQWorkflowCoreOptions = options.Value;

            QueueConfiguration = GetOrCreateWorkflowQueueConfiguration();
        }

        protected virtual WorkflowQueueConfiguration GetOrCreateWorkflowQueueConfiguration()
        {
            return new WorkflowQueueConfiguration(
                RabbitMQWorkflowCoreOptions.DefaultQueueNamePrefix + "Workflow-Core",
                durable: true, 
                exclusive: false, 
                autoDelete: false);
        }

        public async Task<string> DequeueWork(QueueType queue, CancellationToken cancellationToken)
        {
            using (await SyncObj.LockAsync(cancellationToken))
            {
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
            using (await SyncObj.LockAsync())
            {
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
                ChannelPrefix + QueueConfiguration.QueueName,
                QueueConfiguration.ConnectionName
            );

            return Task.CompletedTask;
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
