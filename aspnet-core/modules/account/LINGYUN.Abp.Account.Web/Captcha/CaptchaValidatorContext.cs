using System;

namespace LINGYUN.Abp.Account.Web.Captcha;

public class CaptchaValidatorContext
{
    public IServiceProvider ServiceProvider { get; }
    public string CaptchaCode { get; }
    public CaptchaValidatorContext(
        IServiceProvider serviceProvider,
        string captchaCode)
    {
        ServiceProvider = serviceProvider;
        CaptchaCode = captchaCode;
    }
}
