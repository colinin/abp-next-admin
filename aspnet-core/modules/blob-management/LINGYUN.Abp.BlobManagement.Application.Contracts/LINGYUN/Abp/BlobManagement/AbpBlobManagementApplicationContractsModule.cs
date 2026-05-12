using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BlobManagement;

[DependsOn(
    typeof(AbpBlobManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule))]
public class AbpBlobManagementApplicationContractsModule : AbpModule
{
}
