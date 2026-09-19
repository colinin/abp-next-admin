using LINGYUN.Abp.Account.Web.Captcha;
using LINGYUN.Abp.Captcha;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Account.Web.LazyCaptcha;

public class LazyCaptchaValidator : ICaptchaValidator
{
    public async virtual Task<bool> ValidateAsync(CaptchaValidatorContext context)
    {
        if (!context.CaptchaCode.IsNullOrWhiteSpace())
        {
            var httpContextAccessor = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
            if (httpContextAccessor.HttpContext != null &&
                httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(CaptchaKeywords.CaptchaIdCookieName, out var captchaId))
            {
                var captcha = context.ServiceProvider.GetRequiredService<ICodeCaptchaProvider>();
                return await captcha.ValidateAsync(captchaId, context.CaptchaCode);
            }
        }
        
        return false;
    }
}
