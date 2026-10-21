using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Account.Web.Pages.Account.Components.Captcha;

public abstract class CaptchaViewComponentBase : AbpViewComponent
{
    public abstract Task<IViewComponentResult> InvokeAsync();
}
