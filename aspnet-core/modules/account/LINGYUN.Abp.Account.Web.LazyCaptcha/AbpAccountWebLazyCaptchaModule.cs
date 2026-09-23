using LINGYUN.Abp.Account.Web.Captcha;
using LINGYUN.Abp.Account.Web.LazyCaptcha.Bundling;
using LINGYUN.Abp.Account.Web.LazyCaptcha.Pages.Account.Components.LazyCaptcha;
using LINGYUN.Abp.Account.Web.Pages.Account;
using LINGYUN.Abp.LazyCaptcha;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Account.Web.LazyCaptcha;

[DependsOn(
    typeof(AbpLazyCaptchaModule),
    typeof(AbpAccountWebModule))]
public class AbpAccountWebLazyCaptchaModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(AccountResource), typeof(AbpAccountWebLazyCaptchaModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAccountWebLazyCaptchaModule>("LINGYUN.Abp.Account.Web.LazyCaptcha");
        });

        Configure<AbpAccountCaptchaOptions>(options =>
        {
            options.CaptchaComponents.Add(
                "LazyCaptcha",
                new CaptchaComponent(
                    typeof(LazyCaptchaViewComponent),
                    new LazyCaptchaValidator()));
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles
                .Configure(typeof(LoginModel).FullName!, bundle =>
                {
                    bundle.AddContributors(typeof(LazyCaptchaStyleBundleContributor));
                });
            options.StyleBundles
                .Configure(typeof(RegisterModel).FullName!, bundle =>
                {
                    bundle.AddContributors(typeof(LazyCaptchaStyleBundleContributor));
                });

            options.ScriptBundles
                .Configure(typeof(LoginModel).FullName!, bundle =>
                {
                    bundle.AddContributors(typeof(LazyCaptchaScriptBundleContributor));
                });

            options.ScriptBundles
                .Configure(typeof(RegisterModel).FullName!, bundle =>
                {
                    bundle.AddContributors(typeof(LazyCaptchaScriptBundleContributor));
                });
        });
    }
}
