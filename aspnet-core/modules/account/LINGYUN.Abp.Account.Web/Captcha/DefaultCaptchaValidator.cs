using System.Threading.Tasks;

namespace LINGYUN.Abp.Account.Web.Captcha;

public class DefaultCaptchaValidator : ICaptchaValidator
{
    public Task<bool> ValidateAsync(CaptchaValidatorContext context)
    {
        return Task.FromResult(true);
    }
}
