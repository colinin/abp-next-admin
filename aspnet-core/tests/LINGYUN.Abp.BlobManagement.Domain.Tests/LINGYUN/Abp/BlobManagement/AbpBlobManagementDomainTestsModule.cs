using LINGYUN.Abp.Tests;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BlobManagement;

[DependsOn(
    typeof(AbpBlobManagementDomainModule),
    typeof(AbpTestsBaseModule))]
public class AbpBlobManagementDomainTestsModule : AbpModule
{
}
