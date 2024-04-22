using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Linq;
using Volo.Abp.AspNetCore.WebClientInfo;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AspNetCore.WebClientInfo
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    [ExposeServices(
        typeof(IWebClientInfoProvider),
        typeof(HttpContextWebClientInfoProvider))]
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
            IHeaderDictionary requestHeaders = HttpContextAccessor.HttpContext?.Request?.Headers;
            if (requestHeaders != null &&
                requestHeaders.TryGetValue(Options.ForwardedForHeaderName, out StringValues headers) == true)
            {
                if (!StringValues.IsNullOrEmpty(headers))
                {
                    return headers.Last();
                }
            }
            return base.GetClientIpAddress();
        }
    }
}
