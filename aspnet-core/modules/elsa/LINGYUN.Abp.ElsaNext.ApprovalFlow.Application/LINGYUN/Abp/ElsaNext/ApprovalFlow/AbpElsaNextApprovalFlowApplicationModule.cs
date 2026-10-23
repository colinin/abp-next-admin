using LINGYUN.Abp.ElsaNext.ApprovalFlow.AntFlowDesign;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow;

[DependsOn(
    typeof(AbpAuthorizationModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpElsaNextApprovalFlowDomainModule),
    typeof(AbpElsaNextApprovalFlowDomainAntFlowDesignModule),
    typeof(AbpElsaNextApprovalFlowApplicationContractsModule))]
public class AbpElsaNextApprovalFlowApplicationModule : AbpModule
{
}
