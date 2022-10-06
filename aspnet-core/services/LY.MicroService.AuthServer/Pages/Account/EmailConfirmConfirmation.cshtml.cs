using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.Account.Web.Pages.Account;

namespace LY.MicroService.AuthServer.Pages.Account;

[AllowAnonymous]
public class EmailConfirmConfirmationModel : AccountPageModel
{
    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; }

    [BindProperty(SupportsGet = true)]
    public string ReturnUrlHash { get; set; }

    public virtual Task<IActionResult> OnGetAsync()
    {
        ReturnUrl = GetRedirectUrl(ReturnUrl, ReturnUrlHash);

        return Task.FromResult<IActionResult>(Page());
    }
}
