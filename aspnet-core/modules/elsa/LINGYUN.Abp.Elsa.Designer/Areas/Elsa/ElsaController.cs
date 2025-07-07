using Microsoft.AspNetCore.Mvc;

namespace LINGYUN.Abp.Elsa.Designer.Areas.Elsa;

public class ElsaController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
