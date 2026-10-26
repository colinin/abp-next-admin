using LINGYUN.Abp.ElsaNext.ApprovalFlow.AntFlowDesign;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow;

[DependsOn(
    typeof(AbpAuthorizationModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpMapperlyModule),
    typeof(AbpElsaNextApprovalFlowDomainModule),
    typeof(AbpElsaNextApprovalFlowDomainAntFlowDesignModule),
    typeof(AbpElsaNextApprovalFlowApplicationContractsModule))]
public class AbpElsaNextApprovalFlowApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<AbpElsaNextApprovalFlowApplicationModule>();
    }
}
