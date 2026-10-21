using LINGYUN.Abp.Account.Web.AliyunCaptcha.Models;
using LINGYUN.Abp.Aliyun.Captcha;
using LINGYUN.Abp.Aliyun.Captcha.Security;
using LINGYUN.Abp.Aliyun.Captcha.Settings;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.Account.Web.AliyunCaptcha.Areas.Account.Controllers;

[Controller]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route($"api/{AccountRemoteServiceConsts.ModuleName}/captcha/aliyun")]
[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
public class TencentCaptchaController : AbpControllerBase
{
    protected ICaptchaRegionProvider CaptchaRegionProvider { get; }
    protected ISceneIdEncryptor SceneIdEncryptor { get; }
    protected ISettingProvider SettingProvider { get; }
    public TencentCaptchaController(
        ICaptchaRegionProvider captchaRegionProvider,
        ISceneIdEncryptor sceneIdEncryptor,
        ISettingProvider settingProvider)
    {
        CaptchaRegionProvider = captchaRegionProvider;
        SceneIdEncryptor = sceneIdEncryptor;
        SettingProvider = settingProvider;
    }

    [HttpGet("config")]
    public async virtual Task<CaptchaConfigModel> GetCaptchaConfigAsync()
    {
        var sceneId = await SettingProvider.GetOrNullAsync(AliyunCaptchaSettingNames.SceneId);
        var prefix = await SettingProvider.GetOrNullAsync(AliyunCaptchaSettingNames.Prefix);
        var region = CaptchaRegionProvider.GetRegion();

        Check.NotNullOrWhiteSpace(sceneId, AliyunCaptchaSettingNames.SceneId);
        Check.NotNullOrWhiteSpace(prefix, AliyunCaptchaSettingNames.Prefix);

        if (!await SettingProvider.IsTrueAsync(AliyunCaptchaSettingNames.UseEncryptedSceneId))
        {
            return new CaptchaConfigModel
            {
                SceneId = sceneId,
                Prefix = prefix,
                Region = region,
            };
        }
        else
        {
            var expireTimeSec = await SettingProvider.GetAsync(AliyunCaptchaSettingNames.EncryptedExpireTimeSec, 300);

            var ekey = await SettingProvider.GetOrNullAsync(AliyunCaptchaSettingNames.EKey);
            Check.NotNullOrWhiteSpace(ekey, AliyunCaptchaSettingNames.EKey);

            var encryptedSceneId = SceneIdEncryptor.Encrypt(sceneId, ekey, expireTimeSec);

            return new CaptchaConfigModel
            {
                SceneId = sceneId,
                Prefix = prefix,
                Region = region,
                EncryptedSceneId = encryptedSceneId,
            };
        }
    }
}
