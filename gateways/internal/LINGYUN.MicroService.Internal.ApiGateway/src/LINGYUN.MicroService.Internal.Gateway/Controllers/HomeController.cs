using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.MicroService.Internal.Gateway.Controllers;

public class HomeController : AbpControllerBase
{
    public IActionResult Index()
    {
        return Redirect("/swagger/index.html");
    }
}
