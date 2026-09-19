using LINGYUN.Abp.Account.Web.Pages.Account.Components.Captcha;
using System;

namespace LINGYUN.Abp.Account.Web.Captcha;

public class AbpAccountCaptchaOptions
{
    public Type ComponentType { get; set; }
    public ICaptchaValidator CaptchaValidator { get; set; }
    public AbpAccountCaptchaOptions()
    {
        ComponentType = typeof(NullCaptchaViewComponent);
        CaptchaValidator = new DefaultCaptchaValidator();
    }
}
