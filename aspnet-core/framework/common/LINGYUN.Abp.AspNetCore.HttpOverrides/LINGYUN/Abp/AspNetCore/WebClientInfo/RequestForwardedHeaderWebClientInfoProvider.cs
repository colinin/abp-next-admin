using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using Volo.Abp.AspNetCore.WebClientInfo;

namespace LINGYUN.Abp.AspNetCore.WebClientInfo;

public class RequestForwardedHeaderWebClientInfoProvider : HttpContextWebClientInfoProvider
{
    protected ForwardedHeadersOptions Options { get; }
    public RequestForwardedHeaderWebClientInfoProvider(
        ILogger<HttpContextWebClientInfoProvider> logger, 
        IOptions<ForwardedHeadersOptions> options,
        IHttpContextAccessor httpContextAccessor) 
        : base(logger, httpContextAccessor)
    {
        Options = options.Value;
    }

    protected override string GetClientIpAddress()
    {
        string forwardedForHeader = null;
        var requestHeaders = HttpContextAccessor.HttpContext?.Request?.Headers;
        if (requestHeaders != null &&
            Options.ForwardedHeaders.HasFlag(ForwardedHeaders.XForwardedFor) &&
            requestHeaders.TryGetValue(Options.ForwardedForHeaderName, out StringValues headers) == true)
        {
            var headerStr = headers.ToString();
            if (!headerStr.IsNullOrWhiteSpace())
            {
                // 原始客户端IP永远是第一个
                forwardedForHeader = headerStr.Split(",").First();
            }
        }
        return forwardedForHeader.IsNullOrWhiteSpace() ? base.GetClientIpAddress() : forwardedForHeader;
    }
}
