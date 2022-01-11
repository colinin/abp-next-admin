using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace LY.MicroService.TaskManagement.Controllers;

public class HomeController : AbpController
{
    public IActionResult Index()
    {
        return Redirect("/swagger/index.html");
    }
}
