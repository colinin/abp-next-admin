using LINGYUN.Abp.Aliyun.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Aliyun.Captcha;

[DependsOn(typeof(AbpAliyunModule))]
public class AbpAliyunCaptchaModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAliyunCaptchaModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                   .Get<AliyunResource>()
                   .AddVirtualJson("/LINGYUN/Abp/Aliyun/Captcha/Localization/Resources");
        });
    }
}
