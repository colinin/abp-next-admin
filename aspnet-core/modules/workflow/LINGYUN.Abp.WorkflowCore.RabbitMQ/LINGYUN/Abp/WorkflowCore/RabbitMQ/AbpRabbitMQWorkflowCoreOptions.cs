namespace LINGYUN.Abp.WorkflowCore.RabbitMQ
{
    public class AbpRabbitMQWorkflowCoreOptions
    {
        /// <summary>
        /// Default value: "AbpWorkflows.".
        /// </summary>
        public string DefaultQueueNamePrefix { get; set; }

        /// <summary>
        /// Default value: "AbpWorkflowCore".
        /// </summary>
        public string DefaultConnectionName { get; set; }
        /// <summary>
        /// Default valu: "AbpWorkflowCore"
        /// </summary>
        public string DefaultChannelName { get; set; }

        public AbpRabbitMQWorkflowCoreOptions()
        {
            DefaultQueueNamePrefix = "AbpWorkflows.";
            DefaultConnectionName = "AbpWorkflowCore";
            DefaultChannelName = "AbpWorkflowCore";
        }
    }
}
