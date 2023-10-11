using LINGYUN.Abp.BlobStoring.Nexus;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OssManagement.Nexus;

[DependsOn(
    typeof(AbpOssManagementNexusModule),
    typeof(AbpBlobStoringNexusTestModule))]
public class AbpOssManagementNexusTestsModule : AbpModule
{

}
