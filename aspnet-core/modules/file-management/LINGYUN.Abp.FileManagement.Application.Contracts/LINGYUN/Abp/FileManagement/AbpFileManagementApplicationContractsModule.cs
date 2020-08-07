using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.FileManagement
{
    [DependsOn(
        typeof(AbpDddApplicationModule))]
    public class AbpFileManagementApplicationContractsModule : AbpModule
    {
    }
}
