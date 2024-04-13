using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace LY.MicroService.LocalizationManagement.Controllers;

public class HomeController : AbpControllerBase
{
    public ActionResult Index()
    {
        return Redirect("~/swagger");
    }
}
