using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace LY.MicroService.WorkflowManagement.Controllers;

public class HomeController : AbpController
{
    public IActionResult Index()
    {
        return Redirect("/swagger/index.html");
    }
}
