using LINGYUN.Abp.Tencent.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Tencent.Captcha;

[DependsOn(typeof(AbpTencentCloudModule))]
public class AbpTencentCaptchaModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpTencentCaptchaModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                   .Get<TencentCloudResource>()
                   .AddVirtualJson("/LINGYUN/Abp/Tencent/Captcha/Localization/Resources");
        });
    }
}
