using AlibabaCloud.SDK.Captcha20230305.Models;
using LINGYUN.Abp.Account.Web.Captcha;
using LINGYUN.Abp.Aliyun.Captcha;
using LINGYUN.Abp.Aliyun.Captcha.Settings;
using LINGYUN.Abp.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Tea;
using Volo.Abp;
using Volo.Abp.AspNetCore.WebClientInfo;
using Volo.Abp.Clients;
using Volo.Abp.Identity;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Account.Web.AliyunCaptcha;

public class AliyunCaptchaValidator : ICaptchaValidator
{
    public async virtual Task<bool> ValidateAsync(CaptchaValidatorContext context)
    {
        var identitySecurityLogManager = context.ServiceProvider.GetRequiredService<IdentitySecurityLogManager>();
        var currentClient = context.ServiceProvider.GetRequiredService<ICurrentClient>();
        var logContext = new IdentitySecurityLogContext
        {
            Identity = IdentitySecurityLogIdentityConsts.Identity,
            ClientId = currentClient.Id,
            UserName = context.UserName,
        };
        logContext.WithProperty("Captcha", "AliyunCaptcha");
        var logger = context.ServiceProvider.GetService<ILogger<AliyunCaptchaValidator>>();
        try
        {
            if (context.CaptchaCode.IsNullOrWhiteSpace())
            {
                logContext.Action = IdentitySecurityLogExtendActionConsts.CaptchaFailed;
                logContext.WithProperty("CaptchaCode", "CaptchaCode is invalid");
                return false;
            }

            logContext.WithProperty("CaptchaCode", context.CaptchaCode);
            var webClientInfoProvider = context.ServiceProvider.GetRequiredService<IWebClientInfoProvider>();
            var clientFactory = context.ServiceProvider.GetRequiredService<ICaptchaClientFactory>();
            var settingProvider = context.ServiceProvider.GetRequiredService<ISettingProvider>();

            var sceneId = await settingProvider.GetOrNullAsync(AliyunCaptchaSettingNames.SceneId);

            Check.NotNullOrWhiteSpace(sceneId, AliyunCaptchaSettingNames.SceneId);

            var client = await clientFactory.CreateAsync();

            var response = await client.VerifyIntelligentCaptchaAsync(
                new VerifyIntelligentCaptchaRequest
                {
                    SceneId = sceneId,
                    CaptchaVerifyParam = context.CaptchaCode,
                });

            logContext.WithProperty("StatusCode", response.StatusCode);
            logContext.WithProperty("RequestId", response.Body.RequestId);
            logContext.WithProperty("Success", response.Body.Success);
            logContext.WithProperty("Code", response.Body.Code);
            logContext.WithProperty("Message", response.Body.Message);
            logContext.WithProperty("VerifyResult", response.Body.Result.VerifyResult);
            logContext.WithProperty("VerifyCode", response.Body.Result.VerifyCode);
            logContext.WithProperty("CertifyId", response.Body.Result.CertifyId);

            if (response.StatusCode >= 500)
            {
                // 业务容灾, 请求阿里云失败时默认验证通过
                logContext.Action = IdentitySecurityLogExtendActionConsts.CaptchaSucceeded;
                return true;
            }
            if (response.Body.Success == true && response.Body.Result.VerifyResult == true)
            {
                logContext.Action = IdentitySecurityLogExtendActionConsts.CaptchaSucceeded;
                return true;
            }
            logContext.Action = IdentitySecurityLogExtendActionConsts.CaptchaFailed;
            logger?.LogWarning("Aliyun Cloud captcha valid failed, RequestId: {requestId}, Error: {code}",
                    response.Body.RequestId, response.Body.Result.VerifyCode);
        }
        catch (TeaException ex)
        {
            logContext.Action = IdentitySecurityLogExtendActionConsts.CaptchaError;
            logger?.LogWarning(ex, "Error occurred while invoking Aliyun Cloud captcha service, Error: {code}: {message}",
                    ex.Code, ex.Message);
        }
        catch (Exception ex)
        {
            logContext.Action = IdentitySecurityLogExtendActionConsts.CaptchaError;
            logger?.LogWarning(ex, "Error occurred while invoking Aliyun Cloud captcha service, Error: {message}", ex.Message);
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
