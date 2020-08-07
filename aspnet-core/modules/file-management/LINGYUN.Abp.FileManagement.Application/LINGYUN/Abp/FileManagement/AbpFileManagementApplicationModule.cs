using Volo.Abp.Modularity;

namespace LINGYUN.Abp.FileManagement
{
    [DependsOn(
        typeof(AbpFileManagementDomainModule),
        typeof(AbpFileManagementApplicationContractsModule))]
    public class AbpFileManagementApplicationModule : AbpModule
    {

    }
}
