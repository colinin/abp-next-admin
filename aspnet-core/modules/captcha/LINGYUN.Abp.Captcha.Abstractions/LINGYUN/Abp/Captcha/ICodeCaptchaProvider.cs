using System.Threading.Tasks;

namespace LINGYUN.Abp.Captcha;

public interface ICodeCaptchaProvider
{
    string Name { get; }

    Task<CodeCaptchaData> GenerateAsync(string captchaId);

    Task<bool> ValidateAsync(string captchaId, string code);

    Task<bool> RemoveAsync(string captchaId);
}
