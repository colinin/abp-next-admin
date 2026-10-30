using LINGYUN.Abp.Tests;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BlobManagement;

[DependsOn(
    typeof(AbpBlobManagementDomainModule),
    typeof(AbpTestsBaseModule))]
public class AbpBlobManagementDomainTestsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBlobManagementOptions>(options =>
        {
            options.DownloadKeySecret = "unit-test-download-key-secret";
        });
    }
}
