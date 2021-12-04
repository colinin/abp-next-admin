using Volo.Abp.RabbitMQ;

namespace LINGYUN.Abp.WorkflowCore.RabbitMQ
{
    public class WorkflowQueueConfiguration : QueueDeclareConfiguration
    {
        public WorkflowQueueConfiguration(
            string queueName,
            bool durable = true, 
            bool exclusive = false, 
            bool autoDelete = false, 
            string deadLetterQueueName = null) 
            : base(queueName, durable, exclusive, autoDelete, deadLetterQueueName)
        {
        }
    }
}
