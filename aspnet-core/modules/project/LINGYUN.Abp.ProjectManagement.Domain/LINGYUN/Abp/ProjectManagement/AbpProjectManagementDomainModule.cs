using Volo.Abp.Cli;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ProjectManagement
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(AbpCliCoreModule),
        typeof(AbpProjectManagementDomainSharedModule))]
    public class AbpProjectManagementDomainModule : AbpModule
    {
    }
}
