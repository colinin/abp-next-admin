using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Hangfire
{
    public class HangfireJwtTokenMiddleware : IMiddleware, ITransientDependency
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // 通过 iframe 加载页面的话,需要手动传递 access_token 到参数列表
            if (context.Request.Path.StartsWithSegments("/hangfire") && context.User.Identity?.IsAuthenticated != true)
            {
                if (context.Request.Query.TryGetValue("access_token", out var accessTokens))
                {
                    context.Request.Headers.Add("Authorization", accessTokens);
                }
                var options = context.RequestServices.GetService<IOptions<HangfireDashboardRouteOptions>>()?.Value;
                if (options != null && options.AllowFrameOrigins.Count > 0)
                {
                    // 跨域 iframe
                    context.Response.Headers.TryAdd("X-Frame-Options", $"\"ALLOW-FROM {options.AllowFrameOrigins.JoinAsString(",")}\"");
                }
            }
            await next(context);
        }
    }
}
