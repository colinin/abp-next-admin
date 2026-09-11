using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LINGYUN.Abp.MicroService.WorkflowService.Pages.Auth;

/// <summary>
/// Signs out the local cookie and ends the OIDC session at the external AuthServer.
/// </summary>
public class LogoutModel : PageModel
{
    public IActionResult OnGet(string returnUrl = null)
    {
        var redirectUri = string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl;
        return SignOut(
            new AuthenticationProperties { RedirectUri = redirectUri },
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme);
    }
}
