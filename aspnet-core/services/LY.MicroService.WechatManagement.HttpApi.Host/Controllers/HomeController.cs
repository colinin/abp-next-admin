using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace LY.MicroService.WechatManagement.Controllers;

public class HomeController : AbpControllerBase
{
    public IActionResult Index()
    {
        return Redirect("/swagger/index.html");
    }
}
