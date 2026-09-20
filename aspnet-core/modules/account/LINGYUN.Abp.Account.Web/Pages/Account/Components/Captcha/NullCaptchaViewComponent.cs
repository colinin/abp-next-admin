using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Account.Web.Pages.Account.Components.Captcha;

public class NullCaptchaViewComponent : CaptchaViewComponentBase
{
    public override Task<IViewComponentResult> InvokeAsync(CaptchaViewComponentModel model)
    {
        return Task.FromResult<IViewComponentResult>(View("~/Pages/Account/Components/Captcha/Default.cshtml", model));
    }
}
