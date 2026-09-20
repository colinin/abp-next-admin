using LINGYUN.Abp.Account.Web.Captcha;
using LINGYUN.Abp.Captcha;
using LINGYUN.Abp.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.Clients;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Account.Web.LazyCaptcha;

public class LazyCaptchaValidator : ICaptchaValidator
{
    public async virtual Task<bool> ValidateAsync(CaptchaValidatorContext context)
    {
        var identitySecurityLogManager = context.ServiceProvider.GetRequiredService<IdentitySecurityLogManager>();
        var httpContextAccessor = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        var currentClient = context.ServiceProvider.GetRequiredService<ICurrentClient>();
        var logger = context.ServiceProvider.GetService<ILogger<LazyCaptchaValidator>>();

        var logContext = new IdentitySecurityLogContext
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            ClientId = currentClient.Id,
            UserName = context.UserName,
        };
        logContext.WithProperty("Captcha", "LazyCaptcha");

        try
        {
            if (!context.CaptchaCode.IsNullOrWhiteSpace() &&
                httpContextAccessor.HttpContext != null &&
                httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(CaptchaKeywords.CaptchaIdCookieName, out var captchaId))
            {
                logContext.WithProperty("CaptchaId", captchaId);
                var captcha = context.ServiceProvider.GetRequiredService<ICodeCaptchaProvider>();
                if (await captcha.ValidateAsync(captchaId, context.CaptchaCode))
                {
                    logContext.Action = IdentitySecurityLogExtendActionConsts.CaptchaSucceeded;
                    return true;
                }
            }
            logContext.WithProperty("CaptchaId", "captchaId is invalid");
            logContext.Action = IdentitySecurityLogExtendActionConsts.CaptchaFailed;
        }
        catch (Exception ex)
        {
            logContext.Action = IdentitySecurityLogExtendActionConsts.CaptchaError;
            logger?.LogWarning(ex, "Error occurred while invoking Lazy captcha service.");
        }
        finally
        {
            try
            {
                await identitySecurityLogManager.SaveAsync(logContext);
            }
            catch (Exception ex)
            {
                logger?.LogWarning(ex, "Failed to write the captcha security log!");
            }
        }

        return false;
    }
}
