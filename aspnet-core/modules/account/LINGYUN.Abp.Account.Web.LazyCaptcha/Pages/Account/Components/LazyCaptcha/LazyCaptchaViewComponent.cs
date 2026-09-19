using LINGYUN.Abp.Account.Web.Pages.Account.Components.Captcha;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Account.Web.LazyCaptcha.Pages.Account.Components.LazyCaptcha;

public class LazyCaptchaViewComponent : CaptchaViewComponentBase
{
    public override Task<IViewComponentResult> InvokeAsync(CaptchaViewComponentModel model)
    {
        return Task.FromResult<IViewComponentResult>(View(
            "~/Pages/Account/Components/LazyCaptcha/Default.cshtml",
            new LazyCaptchaViewComponentModel
            {
                PasswordLoginInput = model.PasswordLoginInput,
            }));
    }
}
