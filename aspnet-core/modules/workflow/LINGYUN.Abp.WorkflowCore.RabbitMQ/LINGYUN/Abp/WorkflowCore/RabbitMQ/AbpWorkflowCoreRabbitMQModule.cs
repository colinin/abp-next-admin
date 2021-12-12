using Volo.Abp.Modularity;
using Volo.Abp.RabbitMQ;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.WorkflowCore.RabbitMQ
{
    [DependsOn(typeof(AbpUnitOfWorkModule))]
    [DependsOn(typeof(AbpRabbitMqModule))]
    [DependsOn(typeof(AbpWorkflowCoreModule))]
    public class AbpWorkflowCoreRabbitMQModule : AbpModule
    {
    }
}
