using Volo.Abp.Emailing;
using Volo.Abp.Modularity;
using Volo.Abp.Sms;

namespace LINGYUN.Abp.WorkflowCore.Components
{
    [DependsOn(
        typeof(AbpSmsModule),
        typeof(AbpEmailingModule),
        typeof(AbpWorkflowCoreModule))]
    public class AbpWorkflowCoreComponentsModule : AbpModule
    {
    }
}
