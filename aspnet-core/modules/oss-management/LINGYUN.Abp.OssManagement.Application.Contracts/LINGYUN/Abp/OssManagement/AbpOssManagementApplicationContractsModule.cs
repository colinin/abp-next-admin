using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OssManagement
{
    [DependsOn(
        typeof(AbpOssManagementDomainSharedModule),
        typeof(AbpDddApplicationModule))]
    public class AbpOssManagementApplicationContractsModule : AbpModule
    {
    }
}
