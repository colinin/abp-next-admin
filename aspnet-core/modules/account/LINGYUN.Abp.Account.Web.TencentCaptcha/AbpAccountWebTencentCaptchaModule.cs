using LINGYUN.Abp.Account.Web.Captcha;
using LINGYUN.Abp.Account.Web.Pages.Account;
using LINGYUN.Abp.Account.Web.TencentCaptcha.Bundling;
using LINGYUN.Abp.Account.Web.TencentCaptcha.Pages.Account.Components.TencentCaptcha;
using LINGYUN.Abp.Tencent.Captcha;
using LINGYUN.Abp.Tencent.Localization;
using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Account.Web.TencentCaptcha;

[DependsOn(
    typeof(AbpTencentCaptchaModule),
    typeof(AbpAccountWebModule))]
public class AbpAccountWebTencentCaptchaModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAccountWebTencentCaptchaModule>("LINGYUN.Abp.Account.Web.TencentCaptcha");
        });

        Configure<AbpAccountCaptchaOptions>(options =>
        {
            options.CaptchaComponents.Add(
                "TencentCaptcha",
                new CaptchaComponent(
                    typeof(TencentCaptchaViewComponent),
                    new TencentCaptchaValidator()));
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options.ScriptBundles
                .Configure(typeof(LoginModel).FullName!, bundle =>
                {
                    bundle.AddContributors(typeof(TencentCaptchaScriptBundleContributor));
                });
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AccountResource>()
                .AddVirtualJson("/Localization/Resources/TencentCaptcha");

            options.Resources
                .Get<TencentCloudResource>()
                .AddVirtualJson("/Localization/Resources/TencentCaptcha");
        });
    }
}
