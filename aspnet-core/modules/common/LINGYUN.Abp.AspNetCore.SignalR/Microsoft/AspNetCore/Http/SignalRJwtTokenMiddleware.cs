using LINGYUN.Abp.AspNetCore.SignalR.JwtToken;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Microsoft.AspNetCore.Http
{
    public class SignalRJwtTokenMiddleware : IMiddleware, ITransientDependency
    {
        protected AbpAspNetCoreSignalRJwtTokenMapPathOptions Options { get; }

        public SignalRJwtTokenMiddleware(IOptions<AbpAspNetCoreSignalRJwtTokenMapPathOptions> options)
        {
            Options = options.Value;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (Options.MapJwtTokenPaths.Any(path => context.Request.Path.StartsWithSegments(path)))
            {
                if (context.User.Identity?.IsAuthenticated != true)
                {
                    if (context.Request.Query.TryGetValue("access_token", out var accessToken))
                    {
                        context.Request.Headers.Add("Authorization", $"Bearer {accessToken}");
                    }

                }
            }
            //if (context.Request.Path.StartsWithSegments("/signalr-hubs/"))
            //{
            //    if (context.User.Identity?.IsAuthenticated != true)
            //    {
            //        if (context.Request.Query.TryGetValue("access_token", out var accessToken))
            //        {
            //            context.Request.Headers.Add("Authorization", $"Bearer {accessToken}");
            //        }

            //    }
            //}
            await next(context);
        }
    }
}
