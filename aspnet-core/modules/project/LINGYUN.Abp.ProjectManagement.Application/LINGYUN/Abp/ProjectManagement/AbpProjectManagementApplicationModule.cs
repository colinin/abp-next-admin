using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ProjectManagement
{
    [DependsOn(
        typeof(AbpDddApplicationModule),
        typeof(AbpProjectManagementApplicationContractsModule))]
    public class AbpProjectManagementApplicationModule : AbpModule
    {
    }
}
