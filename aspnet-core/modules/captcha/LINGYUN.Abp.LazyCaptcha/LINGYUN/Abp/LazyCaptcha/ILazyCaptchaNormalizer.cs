namespace LINGYUN.Abp.LazyCaptcha;

public interface ILazyCaptchaNormalizer
{
    string NormalizeCaptchaId(string captchaId);

    string NormalizeRateLimitKey(string captchaId);
}
