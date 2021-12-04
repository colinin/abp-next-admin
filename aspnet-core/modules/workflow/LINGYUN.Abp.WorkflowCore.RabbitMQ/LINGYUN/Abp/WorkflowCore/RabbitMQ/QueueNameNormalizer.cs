using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using WorkflowCore.Interface;

namespace LINGYUN.Abp.WorkflowCore.RabbitMQ
{
    public class QueueNameNormalizer : IQueueNameNormalizer, ISingletonDependency
    {
        protected AbpRabbitMQWorkflowCoreOptions RabbitMQWorkflowCoreOptions { get; }

        public QueueNameNormalizer(
            IOptions<AbpRabbitMQWorkflowCoreOptions> options)
        {
            RabbitMQWorkflowCoreOptions = options.Value;
        }

        public string NormalizeKey(QueueType queue)
        {
            switch (queue)
            {
                case QueueType.Workflow:
                    return RabbitMQWorkflowCoreOptions.DefaultQueueNamePrefix + "wfc.workflow_queue";
                case QueueType.Event:
                    return RabbitMQWorkflowCoreOptions.DefaultQueueNamePrefix + "wfc.event_queue";
                case QueueType.Index:
                    return RabbitMQWorkflowCoreOptions.DefaultQueueNamePrefix + "wfc.index_queue";
                default:
                    return null;
            }
        }
    }
}
