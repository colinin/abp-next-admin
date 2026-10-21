using System.Threading.Tasks;

namespace LINGYUN.Abp.Account.Web.Captcha;

public interface ICaptchaValidator
{
    Task<bool> ValidateAsync(CaptchaValidatorContext context);
}
