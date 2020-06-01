using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public class SignalRJwtTokenMiddleware : IMiddleware, ITransientDependency
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // 仅针对自定义的SignalR hub
            if (context.Request.Path.StartsWithSegments("/signalr-hubs/notifications"))
            {
                if (context.User.Identity?.IsAuthenticated != true)
                {
                    if (context.Request.Query.TryGetValue("access_token", out var accessToken))
                    {
                        context.Request.Headers.Add("Authorization", $"Bearer {accessToken}");
                    }

                }
            }
            await next(context);
        }
    }
}
