using Duende.IdentityModel;
using LINGYUN.Abp.Identity.Session;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.Security.Claims;

namespace LINGYUN.Abp.Account.Web.OpenIddict.Services;

public class OpenIddictSessionAuthenticationService : AuthenticationService
{
    protected IIdentitySessionManager IdentitySessionManager { get; }

    public OpenIddictSessionAuthenticationService(
        IAuthenticationSchemeProvider schemes,
        IAuthenticationHandlerProvider handlers,
        IClaimsTransformation transform,
        IOptions<AuthenticationOptions> options,
        IIdentitySessionManager identitySessionManager) : base(schemes, handlers, transform, options)
    {
        IdentitySessionManager = identitySessionManager;
    }

    public async override Task SignInAsync(HttpContext context, string? scheme, ClaimsPrincipal principal, AuthenticationProperties? properties)
    {
        var claimsIdentity = principal.Identities.FirstOrDefault();
        if (claimsIdentity == null || !claimsIdentity.FindSessionId().IsNullOrWhiteSpace())
        {
            await base.SignInAsync(context, scheme, principal, properties);
            return;
        }

        claimsIdentity.AddClaim(new Claim(AbpClaimTypes.SessionId, Guid.NewGuid().ToString()));

        var clientId = principal.FindClientId();
        if (clientId.IsNullOrWhiteSpace())
        {
            // 从授权回调中获取客户端Id
            clientId = FindClientId(context);
        }

        await base.SignInAsync(context, scheme, principal, properties);

        await IdentitySessionManager.SaveSessionAsync(clientId, principal, context.RequestAborted);
    }

    public async override Task SignOutAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
    {
        var sessionId = context.User?.FindSessionId();
        if (!sessionId.IsNullOrWhiteSpace())
        {
            // Revoke the user session.
            await IdentitySessionManager.RevokeSessionAsync(sessionId, context.RequestAborted);
        }

        await base.SignOutAsync(context, scheme, properties);
    }

    private static string? FindClientId(HttpContext context)
    {
        if (context.Request.Query.TryGetValue("ReturnUrl", out var queryReturnUrls))
        {
            var clientId = GetQueryValue(queryReturnUrls.FirstOrDefault());
            if (!clientId.IsNullOrWhiteSpace())
            {
                return clientId;
            }
        }

        if (context.Request.Form.TryGetValue("ReturnUrl", out var formReturnUrls))
        {
            var clientId = GetQueryValue(formReturnUrls.FirstOrDefault());
            if (!clientId.IsNullOrWhiteSpace())
            {
                return clientId;
            }
        }

        if (context.Request.Query.TryGetValue(JwtClaimTypes.ClientId, out var queryClientIds))
        {
            return queryClientIds.FirstOrDefault();
        }

        if (context.Request.Form.TryGetValue(JwtClaimTypes.ClientId, out var formClientIds))
        {
            return formClientIds.FirstOrDefault();
        }

        return null;
    }

    private static string? GetQueryValue(string? returnUrls)
    {
        if (string.IsNullOrWhiteSpace(returnUrls))
        {
            return null;
        }

        if (returnUrls.Contains('?'))
        {
            returnUrls = returnUrls.Split('?')[1];
        }
        
        var parameters = QueryHelpers.ParseQuery(returnUrls);

        return parameters.TryGetValue(JwtClaimTypes.ClientId, out var clientId) ? clientId.ToString() : null;
    }
}
