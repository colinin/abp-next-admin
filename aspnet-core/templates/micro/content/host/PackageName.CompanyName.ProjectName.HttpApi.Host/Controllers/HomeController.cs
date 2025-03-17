using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace PackageName.CompanyName.ProjectName.Controllers;

public class HomeController : AbpController
{
    public IActionResult Index()
    {
        return Redirect("/swagger/index.html");
    }
}
