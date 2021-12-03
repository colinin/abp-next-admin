using WorkflowCore.Interface;

namespace LINGYUN.Abp.WorkflowCore.RabbitMQ
{
    public interface IQueueNameNormalizer
    {
        string NormalizeKey(QueueType queue);
    }
}
