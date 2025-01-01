using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Account.Web.Pages.Account;

namespace LY.AIO.Applications.Single.Pages.Account;

[AllowAnonymous]
public class EmailConfirmConfirmationModel : AccountPageModel
{
    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; }

    [BindProperty(SupportsGet = true)]
    public string ReturnUrlHash { get; set; }

    public async virtual Task<IActionResult> OnGetAsync()
    {
        ReturnUrl = await GetRedirectUrlAsync(ReturnUrl, ReturnUrlHash);

        return Page();
    }
}
