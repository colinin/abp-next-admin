using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace LY.MicroService.WebhooksManagement.Controllers;

public class HomeController : AbpControllerBase
{
    public IActionResult Index()
    {
        return Redirect("/swagger/index.html");
    }
}
