using Microsoft.AspNetCore.Mvc;

namespace LINGYUN.Abp.MicroService.AllInOne.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
