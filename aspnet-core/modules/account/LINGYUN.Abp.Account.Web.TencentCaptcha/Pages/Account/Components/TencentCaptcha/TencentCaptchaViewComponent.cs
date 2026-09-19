using LINGYUN.Abp.Account.Web.Pages.Account.Components.Captcha;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Account.Web.TencentCaptcha.Pages.Account.Components.TencentCaptcha;

public class TencentCaptchaViewComponent : CaptchaViewComponentBase
{
    public override Task<IViewComponentResult> InvokeAsync(CaptchaViewComponentModel model)
    {
        return Task.FromResult<IViewComponentResult>(View(
            "~/Pages/Account/Components/TencentCaptcha/Default.cshtml",
            new TencentCaptchaViewComponentModel
            {
                PasswordLoginInput = model.PasswordLoginInput
            }));
    }
}
