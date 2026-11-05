using Volo.Abp.AspNetCore.WebClientInfo;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.OperationRateLimiting.AspNetCore;

/// <summary>
/// Resolves the client IP address from the current ASP.NET Core request.
/// </summary>
public class HttpContextOperationRateLimitingClientInfoProvider :
    IOperationRateLimitingClientInfoProvider,
    ITransientDependency
{
    protected IWebClientInfoProvider WebClientInfoProvider { get; }

    public HttpContextOperationRateLimitingClientInfoProvider(IWebClientInfoProvider webClientInfoProvider)
    {
        WebClientInfoProvider = webClientInfoProvider;
    }

    public string? GetClientIpAddress()
    {
        return WebClientInfoProvider.ClientIpAddress;
    }
}
