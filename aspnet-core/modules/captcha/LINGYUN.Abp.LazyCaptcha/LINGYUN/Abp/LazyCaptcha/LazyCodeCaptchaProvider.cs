using Lazy.Captcha.Core;
using Lazy.Captcha.Core.Storage;
using LINGYUN.Abp.Captcha;
using System.Threading.Tasks;

namespace LINGYUN.Abp.LazyCaptcha;

public class LazyCodeCaptchaProvider(
    ICaptcha _captcha,
    IStorage _storage,
    ILazyCaptchaNormalizer _captchaIdNormalizer) : ICodeCaptchaProvider
{
    public const string ProviderName = "LazyCaptcha";
    public string Name => ProviderName;

    public async virtual Task<CodeCaptchaData> GenerateAsync(string captchaId)
    {
        var captchData = _captcha.GenerateWithRateLimit(
            NormalizeCaptchaId(captchaId),
            NormalizeRateLimitKey(captchaId));
        return new CodeCaptchaData(captchData.Id, captchData.Code, captchData.Bytes);
    }

    public virtual Task<bool> RemoveAsync(string captchaId)
    {
        _storage.Remove(NormalizeCaptchaId(captchaId));

        return Task.FromResult(true);
    }

    public async virtual Task<bool> ValidateAsync(string captchaId, string code)
    {
        return _captcha.Validate(NormalizeCaptchaId(captchaId), code);
    }

    protected virtual string NormalizeCaptchaId(string captchaId)
    {
        return _captchaIdNormalizer.NormalizeCaptchaId(captchaId);
    }

    protected virtual string NormalizeRateLimitKey(string captchaId)
    {
        return _captchaIdNormalizer.NormalizeRateLimitKey(captchaId);
    }
}
