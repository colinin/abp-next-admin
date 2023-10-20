using LINGYUN.Abp.Tencent;
using LINGYUN.Abp.Tencent.Localization;
using Volo.Abp.BlobStoring;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.BlobStoring.Tencent;

[DependsOn(
    typeof(AbpBlobStoringModule),
    typeof(AbpTencentCloudModule))]
public class AbpBlobStoringTencentCloudModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpBlobStoringTencentCloudModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<TencentCloudResource>()
                .AddVirtualJson("/LINGYUN/Abp/BlobStoring/Tencent/Localization");
        });
    }
}