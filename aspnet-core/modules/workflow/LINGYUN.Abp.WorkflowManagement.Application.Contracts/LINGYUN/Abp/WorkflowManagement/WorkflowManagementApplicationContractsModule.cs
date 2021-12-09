using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WorkflowManagement
{
    [DependsOn(
        typeof(AbpAuthorizationModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(WorkflowManagementDomainSharedModule))]
    public class WorkflowManagementApplicationContractsModule : AbpModule
    {
    }
}
