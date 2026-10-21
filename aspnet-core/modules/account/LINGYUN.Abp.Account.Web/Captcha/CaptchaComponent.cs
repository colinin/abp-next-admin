using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Account.Web.Captcha;

public class CaptchaComponent
{
    public Type ComponentType { get; }
    public ICaptchaValidator Validator { get; }
    public CaptchaComponent(Type componentType, ICaptchaValidator validator)
    {
        ComponentType = componentType;
        Validator = validator;
    }

    public async virtual Task<bool> ValidateAsync(CaptchaValidatorContext context)
    {
        return await Validator.ValidateAsync(context);
    }
}
