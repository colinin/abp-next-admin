using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LINGYUN.Abp.MicroService.WorkflowService.Pages.Auth;

/// <summary>
/// Triggers the standard OIDC authorization-code flow: signing in challenges the "oidc" scheme so
/// the user is redirected to the external AuthServer, then back to this host's cookie.
/// </summary>
public class LoginModel : PageModel
{
    public IActionResult OnGet(string returnUrl = null)
    {
        var redirectUri = string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl;
        return Challenge(
            new AuthenticationProperties { RedirectUri = redirectUri },
            OpenIdConnectDefaults.AuthenticationScheme);
    }
}
