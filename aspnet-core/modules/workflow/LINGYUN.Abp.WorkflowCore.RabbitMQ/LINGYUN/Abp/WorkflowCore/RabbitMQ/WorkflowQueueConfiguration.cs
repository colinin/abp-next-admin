using Volo.Abp.RabbitMQ;

namespace LINGYUN.Abp.WorkflowCore.RabbitMQ
{
    public class WorkflowQueueConfiguration : QueueDeclareConfiguration
    {
        public string ConnectionName { get; set; }

        public WorkflowQueueConfiguration(
            string queueName,
            string connectionName = null,
            bool durable = true, 
            bool exclusive = false, 
            bool autoDelete = false, 
            string deadLetterQueueName = null) 
            : base(queueName, durable, exclusive, autoDelete, deadLetterQueueName)
        {
            ConnectionName = connectionName;
        }
    }
}
