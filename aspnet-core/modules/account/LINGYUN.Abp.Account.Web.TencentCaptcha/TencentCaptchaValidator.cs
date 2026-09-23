using LINGYUN.Abp.Account.Web.Captcha;
using LINGYUN.Abp.Identity;
using LINGYUN.Abp.Tencent.Captcha;
using LINGYUN.Abp.Tencent.Captcha.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TencentCloud.Captcha.V20190722.Models;
using TencentCloud.Common;
using Volo.Abp;
using Volo.Abp.AspNetCore.WebClientInfo;
using Volo.Abp.Clients;
using Volo.Abp.Identity;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Account.Web.TencentCaptcha;

public class TencentCaptchaValidator : ICaptchaValidator
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
        logContext.WithProperty("Captcha", "TencentCaptcha");
        var logger = context.ServiceProvider.GetService<ILogger<TencentCaptchaValidator>>();

        try
        {
            if (context.CaptchaCode.IsNullOrWhiteSpace())
            {
                logContext.Action = IdentitySecurityLogExtendActionConsts.CaptchaFailed;
                logContext.WithProperty("CaptchaCode", "CaptchaCode is invalid");
                return false;
            }
            var captchaKeys = context.CaptchaCode.Split(';');
            if (captchaKeys.Length <= 1)
            {
                logContext.Action = IdentitySecurityLogExtendActionConsts.CaptchaFailed;
                logContext.WithProperty("CaptchaCode", "CaptchaCode is invalid");
                return false;
            }

            var webClientInfoProvider = context.ServiceProvider.GetRequiredService<IWebClientInfoProvider>();
            var clientFactory = context.ServiceProvider.GetRequiredService<ICaptchaClientFactory>();
            var settingProvider = context.ServiceProvider.GetRequiredService<ISettingProvider>();

            var captchaAppId = await settingProvider.GetOrNullAsync(TencentCaptchaSettingNames.CaptchaAppId);
            var appSecretKey = await settingProvider.GetOrNullAsync(TencentCaptchaSettingNames.AppSecretKey);

            Check.NotNullOrWhiteSpace(captchaAppId, TencentCaptchaSettingNames.CaptchaAppId);
            Check.NotNullOrWhiteSpace(appSecretKey, TencentCaptchaSettingNames.AppSecretKey);

            var client = await clientFactory.CreateAsync();

            var response = await client.DescribeCaptchaResult(
                new DescribeCaptchaResultRequest
                {
                    CaptchaAppId = ulong.Parse(captchaAppId),
                    AppSecretKey = appSecretKey,
                    CaptchaType = 9,
                    Randstr = captchaKeys[0],
                    Ticket = captchaKeys[1],
                    UserIp = webClientInfoProvider.ClientIpAddress,
                });

            logContext.WithProperty("CaptchaCode", response.CaptchaCode);
            logContext.WithProperty("CaptchaMsg", response.CaptchaMsg);
            logContext.WithProperty("EvilLevel", response.EvilLevel);
            logContext.WithProperty("GetCaptchaTime", response.GetCaptchaTime);
            logContext.WithProperty("EvilBitmap", response.EvilBitmap);
            logContext.WithProperty("SubmitCaptchaTime", response.SubmitCaptchaTime);
            logContext.WithProperty("DeviceRiskCategory", response.DeviceRiskCategory);
            logContext.WithProperty("Score", response.Score);
            logContext.WithProperty("RequestId", response.RequestId);

            if (response.CaptchaCode == 1 && response.EvilLevel != 100)
            {
                logContext.Action = IdentitySecurityLogExtendActionConsts.CaptchaSucceeded;
                return true;
            }
            logContext.Action = IdentitySecurityLogExtendActionConsts.CaptchaFailed;
            logger?.LogWarning("Tencent Cloud captcha valid failed, RequestId: {requestId}, Error: {code}: {message}",
                    response.RequestId, response.CaptchaCode, response.CaptchaMsg);
        }
        catch (TencentCloudSDKException ex)
        {
            logger?.LogWarning(ex, "Error occurred while invoking Tencent Cloud captcha service, RequestId: {requestId}, Error: {code}: {message}",
                    ex.RequestId, ex.ErrorCode, ex.Message);
            if (ex.RequestId.IsNullOrWhiteSpace())
            {
                // 业务容灾, 请求腾讯云失败时默认验证通过
                // 参考源码, 网络请求失败时没有RequestId字段
                logContext.Action = IdentitySecurityLogExtendActionConsts.CaptchaSucceeded;
                return true;
            }
            logContext.Action = IdentitySecurityLogExtendActionConsts.CaptchaError;
        }
        catch (Exception ex)
        {
            logContext.Action = IdentitySecurityLogExtendActionConsts.CaptchaError;
            logger?.LogWarning(ex, "Error occurred while invoking Tencent Cloud captcha service, Error: {message}", ex.Message);
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
