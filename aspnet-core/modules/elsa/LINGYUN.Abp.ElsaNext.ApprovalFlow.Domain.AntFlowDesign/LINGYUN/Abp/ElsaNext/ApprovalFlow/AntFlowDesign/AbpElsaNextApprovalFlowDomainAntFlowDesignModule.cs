using LINGYUN.Abp.ElsaNext.ApprovalFlow.Workflows;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.AntFlowDesign;

[DependsOn(
    typeof(AbpElsaNextApprovalFlowDomainModule))]
public class AbpElsaNextApprovalFlowDomainAntFlowDesignModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IWorkflowTemplateCompiler, AntFlowDesignWorkflowTemplateCompiler>();
    }
}
