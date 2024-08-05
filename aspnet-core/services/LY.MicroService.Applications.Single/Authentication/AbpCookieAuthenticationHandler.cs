using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
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
    protected override Task InitializeEventsAsync()
    {
        var events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = ctx =>
            {
                if (ctx.Request.CanAccept(MimeTypes.Application.Json))
                {
                    ctx.Response.Headers.Location = ctx.RedirectUri;
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
                if (ctx.Request.CanAccept(MimeTypes.Application.Json))
                {
                    ctx.Response.Headers.Location = ctx.RedirectUri;
                    ctx.Response.StatusCode = 403;
                }
                else
                {
                    ctx.Response.Redirect(ctx.RedirectUri);
                }
                return Task.CompletedTask;
            },
            OnRedirectToLogout = ctx =>
            {
                if (ctx.Request.CanAccept(MimeTypes.Application.Json))
                {
                    ctx.Response.Headers.Location = ctx.RedirectUri;
                }
                else
                {
                    ctx.Response.Redirect(ctx.RedirectUri);
                }
                return Task.CompletedTask;
            },
            OnRedirectToReturnUrl = ctx =>
            {
                if (ctx.Request.CanAccept(MimeTypes.Application.Json))
                {
                    ctx.Response.Headers.Location = ctx.RedirectUri;
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
