using LINGYUN.Abp.Account.Web.AliyunCaptcha.Bundling;
using LINGYUN.Abp.Account.Web.AliyunCaptcha.Pages.Account.Components.AliyunCaptcha;
using LINGYUN.Abp.Account.Web.Captcha;
using LINGYUN.Abp.Account.Web.Pages.Account;
using LINGYUN.Abp.Aliyun.Captcha;
using LINGYUN.Abp.Aliyun.Localization;
using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Account.Web.AliyunCaptcha;

[DependsOn(
    typeof(AbpAliyunCaptchaModule),
    typeof(AbpAccountWebModule))]
public class AbpAccountWebAliyunCaptchaModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAccountWebAliyunCaptchaModule>("LINGYUN.Abp.Account.Web.AliyunCaptcha");
        });

        Configure<AbpAccountCaptchaOptions>(options =>
        {
            options.CaptchaComponents.Add(
                "AliyunCaptcha",
                new CaptchaComponent(
                    typeof(AliyunCaptchaViewComponent),
                    new AliyunCaptchaValidator()));
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options.ScriptBundles
                .Configure(typeof(LoginModel).FullName!, bundle =>
                {
                    bundle.AddContributors(typeof(AliyunCaptchaScriptBundleContributor));
                });
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AccountResource>()
                .AddVirtualJson("/Localization/Resources/AliyunCaptcha");

            options.Resources
                .Get<AliyunResource>()
                .AddVirtualJson("/Localization/Resources/AliyunCaptcha");
        });
    }
}
