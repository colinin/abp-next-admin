using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace LY.MicroService.PlatformManagement.Controllers;

public class HomeController : AbpControllerBase
{
    public ActionResult Index()
    {
        return Redirect("~/swagger");
    }
}
