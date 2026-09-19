using LINGYUN.Abp.Account.Web.Captcha;
using LINGYUN.Abp.Tencent.Settings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using TencentCloud.Captcha.V20190722;
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
                new TencentCloud.Common.Credential
                {
                    SecretId = secretId,
                    SecretKey = secretKey
                },
                "");

            var response = await client.DescribeCaptchaResult(
                new TencentCloud.Captcha.V20190722.Models.DescribeCaptchaResultRequest
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
        }

        return false;
    }
}
