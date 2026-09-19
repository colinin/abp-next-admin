using LINGYUN.Abp.Account.Web.TencentCaptcha.Models;
using LINGYUN.Abp.Account.Web.TencentCaptcha.Security;
using LINGYUN.Abp.Account.Web.TencentCaptcha.Settings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Account.Web.TencentCaptcha.Areas.Account.Controllers;

[Controller]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route($"api/{AccountRemoteServiceConsts.ModuleName}/captcha")]
[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
public class TencentCaptchaController : AbpControllerBase
{
    protected const long MaxExpireTimeSeconds = 86400L;

    protected ICaptchaAppIdEncryptorFactory CaptchaAppIdEncryptorFactory { get; }
    protected ISettingProvider SettingProvider { get; }
    public TencentCaptchaController(
        ICaptchaAppIdEncryptorFactory captchaAppIdEncryptorFactory,
        ISettingProvider settingProvider)
    {
        CaptchaAppIdEncryptorFactory = captchaAppIdEncryptorFactory;
        SettingProvider = settingProvider;
    }
    [HttpGet("config")]
    public async virtual Task<CaptchaConfigModel> GetCaptchaConfigAsync()
    {
        var captchaAppId = await SettingProvider.GetOrNullAsync(TencentCaptchaSettingNames.CaptchaAppId);

        Check.NotNullOrWhiteSpace(captchaAppId, TencentCaptchaSettingNames.CaptchaAppId);

        var aidEncryptedType = await SettingProvider.GetOrNullAsync(TencentCaptchaSettingNames.CaptchaAppIdEncryptedType);
        if (aidEncryptedType.IsNullOrWhiteSpace())
        {
            return new CaptchaConfigModel
            {
                CaptchaAppId = captchaAppId
            };
        }
        else
        {
            var appSecretKey = await SettingProvider.GetOrNullAsync(TencentCaptchaSettingNames.AppSecretKey);
            Check.NotNullOrWhiteSpace(appSecretKey, TencentCaptchaSettingNames.AppSecretKey);

            var expireTime = await SettingProvider.GetAsync(TencentCaptchaSettingNames.CaptchaAppIdEncryptedExpireTime, 300L);
            if (expireTime <= 0 || expireTime > MaxExpireTimeSeconds)
            {
                expireTime = MaxExpireTimeSeconds;
            }

            var captchaAppIdEncryptor = CaptchaAppIdEncryptorFactory.Create(aidEncryptedType);
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var key = captchaAppIdEncryptor.PadKey(Encoding.UTF8.GetBytes(appSecretKey));
            var plaintext = $"{captchaAppId}&{timestamp}&{expireTime}";

            byte[] iv;
            byte[]? aad = null;
            string? aidEncryptedAad = null;

            if (aidEncryptedType == "gcm")
            {
                iv = RandomNumberGenerator.GetBytes(12);
            }
            else
            {
                iv = RandomNumberGenerator.GetBytes(16);
            }

            var aidEncrypted = captchaAppIdEncryptor.Encrypt(plaintext, key, iv, aad);

            return new CaptchaConfigModel
            {
                CaptchaAppId = captchaAppId,
                AidEncrypted = aidEncrypted,
                AidEncryptedType = aidEncryptedType,
                AidEncryptedAad = aidEncryptedAad
            };
        }
    }
}
