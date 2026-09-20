using LINGYUN.Abp.Account.Web.Captcha;
using LINGYUN.Abp.Tencent.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TencentCloud.Captcha.V20190722;
using TencentCloud.Captcha.V20190722.Models;
using TencentCloud.Common;
using Volo.Abp;
using Volo.Abp.AspNetCore.WebClientInfo;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Account.Web.TencentCaptcha;

public class TencentCaptchaValidator : ICaptchaValidator
{
    public async virtual Task<bool> ValidateAsync(CaptchaValidatorContext context)
    {
        if (!context.CaptchaCode.IsNullOrWhiteSpace())
        {
            var captchaKeys = context.CaptchaCode.Split(';');
            if (captchaKeys.Length <= 1)
            {
                return false;
            }

            var webClientInfoProvider = context.ServiceProvider.GetRequiredService<IWebClientInfoProvider>();
            var settingProvider = context.ServiceProvider.GetRequiredService<ISettingProvider>();

            var tencentSettings = await settingProvider.GetAllAsync(
                [
                    TencentCloudSettingNames.SecretId,
                    TencentCloudSettingNames.SecretKey,
                    TencentCloudSettingNames.Captcha.CaptchaAppId,
                    TencentCloudSettingNames.Captcha.AppSecretKey
                ]);

            var secretId = tencentSettings.FirstOrDefault(x => x.Name == TencentCloudSettingNames.SecretId)?.Value;
            var secretKey = tencentSettings.FirstOrDefault(x => x.Name == TencentCloudSettingNames.SecretKey)?.Value;
            var captchaAppId = tencentSettings.FirstOrDefault(x => x.Name == TencentCloudSettingNames.Captcha.CaptchaAppId)?.Value;
            var appSecretKey = tencentSettings.FirstOrDefault(x => x.Name == TencentCloudSettingNames.Captcha.AppSecretKey)?.Value;

            Check.NotNullOrWhiteSpace(secretId, TencentCloudSettingNames.SecretId);
            Check.NotNullOrWhiteSpace(secretKey, TencentCloudSettingNames.SecretKey);
            Check.NotNullOrWhiteSpace(captchaAppId, TencentCloudSettingNames.Captcha.CaptchaAppId);
            Check.NotNullOrWhiteSpace(appSecretKey, TencentCloudSettingNames.Captcha.AppSecretKey);

            var client = new CaptchaClient(
                new Credential
                {
                    SecretId = secretId,
                    SecretKey = secretKey
                },
                "");

            try
            {
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
                if (response.CaptchaCode == 1)
                {
                    return true;
                }
                context.ServiceProvider
                    .GetService<ILogger<TencentCaptchaValidator>>()
                    ?.LogWarning("Tencent Cloud captcha verification failed, RequestId: {requestId}, Error: {code}: {message}",
                        response.RequestId, response.CaptchaCode, response.CaptchaMsg);
            }
            catch (TencentCloudSDKException ex)
            {
                context.ServiceProvider
                    .GetService<ILogger<TencentCaptchaValidator>>()
                    ?.LogWarning(ex, "Error occurred while invoking Tencent Cloud captcha service, RequestId: {requestId}, Error: {code}: {message}", 
                        ex.RequestId, ex.ErrorCode, ex.Message);
            }
        }

        return false;
    }
}
