using Elsa.Studio.Authentication.Abstractions.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections.Client;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.MicroService.AllInOne;

/// <summary>
/// Passes the host authentication cookie to Elsa Studio SignalR connections.
/// AllInOne hosts Studio and the Elsa API in the same application, so the
/// browser cookie is the active authentication mechanism for real-time tasks.
/// </summary>
public sealed class StudioHttpConnectionOptionsConfigurator : IHttpConnectionOptionsConfigurator
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public StudioHttpConnectionOptionsConfigurator(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task ConfigureAsync(
        HttpConnectionOptions options,
        CancellationToken cancellationToken = default)
    {
        var cookie = _httpContextAccessor.HttpContext?.Request.Headers.Cookie.ToString();
        if (!string.IsNullOrWhiteSpace(cookie))
        {
            options.Headers["Cookie"] = cookie;
        }

        return Task.CompletedTask;
    }
}
