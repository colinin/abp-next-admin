using LINGYUN.Abp.Identity.Settings;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Account.Web.Captcha;

public class CaptchaComponentProvider : ICaptchaComponentProvider, IScopedDependency
{
    protected AbpAccountCaptchaOptions Options { get; }
    protected ISettingProvider SettingProvider { get; }
    public CaptchaComponentProvider(
        IOptions<AbpAccountCaptchaOptions> options,
        ISettingProvider settingProvider)
    {
        Options = options.Value;
        SettingProvider = settingProvider;
    }
    public async virtual Task<CaptchaComponent> GetComponentOrDefaultAsync()
    {
        var captchaComponetSet = await SettingProvider.GetOrNullAsync(IdentitySettingNames.SignIn.CaptchaComponent);

        return Options.CaptchaComponents.GetComponentOrDefault(captchaComponetSet ?? CaptchaComponentDictionary.DefaultName);
    }
}
