using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace LY.MicroService.AuthServer.Controllers;

public class HomeController : AbpControllerBase
{
    public ActionResult Index()
    {
        return Redirect("~/swagger");
    }
}
