namespace LINGYUN.Abp.WorkflowCore.RabbitMQ
{
    public class AbpRabbitMQWorkflowCoreOptions
    {
        /// <summary>
        /// Default value: "AbpWorkflows.".
        /// </summary>
        public string DefaultQueueNamePrefix { get; set; }

        public AbpRabbitMQWorkflowCoreOptions()
        {
            DefaultQueueNamePrefix = "AbpWorkflows.";
        }
    }
}
