using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Account.Web.LazyCaptcha.Bundling;

public class LazyCaptchaScriptBundleContributor : BundleContributor
{
    public async override Task ConfigureBundleAsync(BundleConfigurationContext context)
    {
        var settingProvider = context.LazyServiceProvider.GetRequiredService<ISettingProvider>();
        if (await settingProvider.IsTrueAsync(Identity.Settings.IdentitySettingNames.SignIn.RequireCaptchaVerification) &&
            await settingProvider.GetOrNullAsync(Identity.Settings.IdentitySettingNames.SignIn.CaptchaComponent) == "LazyCaptcha")
        {
            context.Files.AddIfNotContains("/client-proxies/lazy-captcha-proxy.js");
            context.Files.AddIfNotContains("/Pages/Account/Components/LazyCaptcha/Default.js");
        }
    }
}
