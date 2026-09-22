using JetBrains.Annotations;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Account.Web.Captcha;

public interface ICaptchaComponentProvider
{
    [ItemNotNull]
    Task<CaptchaComponent> GetComponentOrDefaultAsync();
}
