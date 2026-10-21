using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Account.Web.AliyunCaptcha.Bundling;

public class AliyunCaptchaScriptBundleContributor : BundleContributor
{
    public async override Task ConfigureBundleAsync(BundleConfigurationContext context)
    {
        var settingProvider = context.LazyServiceProvider.GetRequiredService<ISettingProvider>();
        if (await settingProvider.IsTrueAsync(Identity.Settings.IdentitySettingNames.SignIn.RequireCaptchaVerification) &&
            await settingProvider.GetOrNullAsync(Identity.Settings.IdentitySettingNames.SignIn.CaptchaComponent) == "AliyunCaptcha")
        {
            context.Files.AddIfNotContains("/client-proxies/aliyun-captcha-proxy.js");
            context.Files.AddIfNotContains("https://o.alicdn.com/captcha-frontend/aliyunCaptcha/AliyunCaptcha.js");
            context.Files.AddIfNotContains("/Pages/Account/Components/AliyunCaptcha/Default.js");
        }
    }
}
