using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    [DependsOn(typeof(AbpWorkflowCoreModule))]
    public class AbpWorkflowCorePersistenceModule : AbpModule
    {
    }
}
