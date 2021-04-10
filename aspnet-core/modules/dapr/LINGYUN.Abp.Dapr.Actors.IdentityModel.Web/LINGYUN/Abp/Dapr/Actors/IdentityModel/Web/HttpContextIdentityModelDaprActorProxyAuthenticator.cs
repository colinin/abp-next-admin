using LINGYUN.Abp.Dapr.Actors.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IdentityModel;

namespace LINGYUN.Abp.Dapr.Actors.IdentityModel
{
    [Dependency(ReplaceServices = true)]
    public class HttpContextIdentityModelDaprActorProxyAuthenticator : IdentityModelDaprActorProxyAuthenticator
    {
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public HttpContextIdentityModelDaprActorProxyAuthenticator(
            IIdentityModelAuthenticationService identityModelAuthenticationService)
            : base(identityModelAuthenticationService)
        {
        }

        public override async Task AuthenticateAsync(DaprActorProxyAuthenticateContext context)
        {
            if (context.RemoteService.GetUseCurrentAccessToken() != false)
            {
                var accessToken = await GetAccessTokenFromHttpContextOrNullAsync();
                if (accessToken != null)
                {
                    context.Handler.PreConfigure(request =>
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    });
                    return;
                }
            }

            await base.AuthenticateAsync(context);
        }

        protected virtual async Task<string> GetAccessTokenFromHttpContextOrNullAsync()
        {
            var httpContext = HttpContextAccessor?.HttpContext;
            if (httpContext == null)
            {
                return null;
            }

            return await httpContext.GetTokenAsync("access_token");
        }
    }
}
