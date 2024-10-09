using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Text.Encodings.Web;
using Volo.Abp.Http;

namespace LY.MicroService.Applications.Single.Authentication;

public class AbpCookieAuthenticationHandler : CookieAuthenticationHandler
{
    public AbpCookieAuthenticationHandler(
        IOptionsMonitor<CookieAuthenticationOptions> options, 
        ILoggerFactory logger, 
        UrlEncoder encoder) : base(options, logger, encoder)
    {
    }

    public AbpCookieAuthenticationHandler(
        IOptionsMonitor<CookieAuthenticationOptions> options, 
        ILoggerFactory logger, 
        UrlEncoder encoder, 
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected const string XRequestFromHeader = "X-Request-From";
    protected const string DontRedirectRequestFromHeader = "vben";
    protected override Task InitializeEventsAsync()
    {
        var events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = ctx =>
            {
                if (string.Equals(ctx.Request.Headers[XRequestFromHeader], DontRedirectRequestFromHeader, StringComparison.Ordinal))
                {
                    // ctx.Response.Headers.Location = ctx.RedirectUri;
                    ctx.Response.StatusCode = 401;
                }
                else
                {
                    ctx.Response.Redirect(ctx.RedirectUri);
                }
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = ctx =>
            {
                if (string.Equals(ctx.Request.Headers[XRequestFromHeader], DontRedirectRequestFromHeader, StringComparison.Ordinal))
                {
                    // ctx.Response.Headers.Location = ctx.RedirectUri;
                    ctx.Response.StatusCode = 401;
                }
                else
                {
                    ctx.Response.Redirect(ctx.RedirectUri);
                }
                return Task.CompletedTask;
            },
            OnRedirectToLogout = ctx =>
            {
                if (string.Equals(ctx.Request.Headers[XRequestFromHeader], DontRedirectRequestFromHeader, StringComparison.Ordinal))
                {
                    // ctx.Response.Headers.Location = ctx.RedirectUri;
                    ctx.Response.StatusCode = 401;
                }
                else
                {
                    ctx.Response.Redirect(ctx.RedirectUri);
                }
                return Task.CompletedTask;
            },
            OnRedirectToReturnUrl = ctx =>
            {
                if (string.Equals(ctx.Request.Headers[XRequestFromHeader], DontRedirectRequestFromHeader, StringComparison.Ordinal))
                {
                    // ctx.Response.Headers.Location = ctx.RedirectUri;
                    ctx.Response.StatusCode = 401;
                }
                else
                {
                    ctx.Response.Redirect(ctx.RedirectUri);
                }
                return Task.CompletedTask;
            }
        };

        Events = events;

        return Task.CompletedTask;
    }
}
