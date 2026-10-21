using LINGYUN.Abp.Account.Web.Pages.Account.Components.Captcha;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Account.Web.AliyunCaptcha.Pages.Account.Components.AliyunCaptcha;

public class AliyunCaptchaViewComponent : CaptchaViewComponentBase
{
    public override Task<IViewComponentResult> InvokeAsync()
    {
        return Task.FromResult<IViewComponentResult>(View("~/Pages/Account/Components/AliyunCaptcha/Default.cshtml"));
    }
}
