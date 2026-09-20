using System;

namespace LINGYUN.Abp.Account.Web.Captcha;

public class CaptchaValidatorContext
{
    public IServiceProvider ServiceProvider { get; }
    public string CaptchaCode { get; }
    public string? UserName { get; }
    public CaptchaValidatorContext(
        IServiceProvider serviceProvider,
        string captchaCode,
        string? userName = null)
    {
        ServiceProvider = serviceProvider;
        CaptchaCode = captchaCode;
        UserName = userName;
    }
}
