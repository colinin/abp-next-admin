namespace LINGYUN.Abp.Account.Web.Captcha;

public class AbpAccountCaptchaOptions
{
    public CaptchaComponentDictionary CaptchaComponents { get; }
    public AbpAccountCaptchaOptions()
    {
        CaptchaComponents = new CaptchaComponentDictionary();
    }
}
