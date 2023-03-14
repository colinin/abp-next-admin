using Microsoft.AspNetCore.Mvc;

namespace LY.MicroService.Applications.Single.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return Redirect("/swagger");
    }
}
