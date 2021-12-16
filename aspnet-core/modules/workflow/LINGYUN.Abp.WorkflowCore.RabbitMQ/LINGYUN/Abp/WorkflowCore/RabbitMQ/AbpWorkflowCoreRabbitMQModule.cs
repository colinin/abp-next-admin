using Volo.Abp.Modularity;
using Volo.Abp.RabbitMQ;

namespace LINGYUN.Abp.WorkflowCore.RabbitMQ
{
    [DependsOn(typeof(AbpRabbitMqModule))]
    [DependsOn(typeof(AbpWorkflowCoreModule))]
    public class AbpWorkflowCoreRabbitMQModule : AbpModule
    {
    }
}
