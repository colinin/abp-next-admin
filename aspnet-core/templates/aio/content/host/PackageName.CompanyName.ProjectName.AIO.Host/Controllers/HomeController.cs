using Microsoft.AspNetCore.Mvc;

namespace PackageName.CompanyName.ProjectName.AIO.Host.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return Redirect("/swagger");
    }
}
