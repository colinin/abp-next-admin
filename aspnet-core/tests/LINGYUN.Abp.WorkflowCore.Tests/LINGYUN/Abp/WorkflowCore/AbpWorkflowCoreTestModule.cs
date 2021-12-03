using LINGYUN.Abp.Tests;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WorkflowCore
{
    [DependsOn(typeof(AbpTestsBaseModule))]
    [DependsOn(typeof(AbpWorkflowCoreModule))]
    public class AbpWorkflowCoreTestModule : AbpModule
    {
    }
}
