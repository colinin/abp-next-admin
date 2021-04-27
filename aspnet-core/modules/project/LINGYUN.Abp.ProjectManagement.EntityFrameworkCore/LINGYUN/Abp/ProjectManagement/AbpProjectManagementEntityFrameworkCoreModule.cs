using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.ProjectManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpProjectManagementDomainModule),
        typeof(AbpEntityFrameworkCoreModule))]
    public class AbpProjectManagementEntityFrameworkCoreModule : AbpModule
    {
    }
}
