using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Account.Web.TencentCaptcha.Bundling;

public class TencentCaptchaScriptBundleContributor : BundleContributor
{
    public async override Task ConfigureBundleAsync(BundleConfigurationContext context)
    {
        var settingProvider = context.LazyServiceProvider.GetRequiredService<ISettingProvider>();
        if (await settingProvider.IsTrueAsync(Identity.Settings.IdentitySettingNames.SignIn.RequireCaptchaVerification))
        {
            context.Files.AddIfNotContains("/client-proxies/tencent-captcha-proxy.js");
            context.Files.AddIfNotContains("https://turing.captcha.qcloud.com/TJCaptcha.js");
            context.Files.AddIfNotContains("/Pages/Account/Components/TencentCaptcha/Default.js");
        }
    }
}
