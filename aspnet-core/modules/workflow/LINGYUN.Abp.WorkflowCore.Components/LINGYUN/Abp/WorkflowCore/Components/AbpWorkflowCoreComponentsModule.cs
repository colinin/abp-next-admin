using LINGYUN.Abp.WorkflowCore.Components.Authentication;
using Microsoft.Extensions.DependencyInjection;
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
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton<ICurrentAssigner>(AsyncLocalCurrentAssigner.Instance);
        }
    }
}
