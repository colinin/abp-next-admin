using Volo.Abp.DependencyInjection;
using WorkflowCore.Interface;

namespace LINGYUN.Abp.WorkflowCore.RabbitMQ
{
    public class QueueNameNormalizer : IQueueNameNormalizer, ISingletonDependency
    {
        public string NormalizeKey(QueueType queue)
        {
            switch (queue)
            {
                case QueueType.Workflow:
                    return "wfc.workflow_queue";
                case QueueType.Event:
                    return "wfc.event_queue";
                case QueueType.Index:
                    return "wfc.index_queue";
                default:
                    return null;
            }
        }
    }
}
