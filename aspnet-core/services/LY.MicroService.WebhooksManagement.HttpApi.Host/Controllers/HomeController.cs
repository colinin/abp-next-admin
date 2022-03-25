using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace LY..WebhooksManagement.Controllers;

public class HomeController : AbpController
{
    public IActionResult Index()
    {
        return Redirect("/swagger/index.html");
    }
}
