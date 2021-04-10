using LINGYUN.Abp.Dapr.Actors.Authentication;
using LINGYUN.Abp.Dapr.Actors.DynamicProxying;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IdentityModel;

namespace LINGYUN.Abp.Dapr.Actors.IdentityModel
{
    [Dependency(ReplaceServices = true)]
    public class IdentityModelDaprActorProxyAuthenticator : IDaprActorProxyAuthenticator, ITransientDependency
    {
        protected AbpIdentityClientOptions ClientOptions { get; }
        protected IIdentityModelAuthenticationService IdentityModelAuthenticationService { get; }

        public ILogger<IdentityModelDaprActorProxyAuthenticator> Logger { get; set; }

        public IdentityModelDaprActorProxyAuthenticator(
            IIdentityModelAuthenticationService identityModelAuthenticationService)
        {
            IdentityModelAuthenticationService = identityModelAuthenticationService;
            Logger = NullLogger<IdentityModelDaprActorProxyAuthenticator>.Instance;
        }

        public virtual async Task AuthenticateAsync(DaprActorProxyAuthenticateContext context)
        {
            var identityClientName = context.RemoteService.GetIdentityClient();
            var configuration = GetClientConfiguration(identityClientName);
            if (configuration == null)
            {
                Logger.LogWarning($"Could not find {nameof(IdentityClientConfiguration)} for {identityClientName}. Either define a configuration for {identityClientName} or set a default configuration.");
                return;
            }
            var accessToken = await IdentityModelAuthenticationService.GetAccessTokenAsync(configuration);
            if (accessToken == null)
            {
                return;
            }

            SetAccessToken(context.Handler, accessToken);
        }

        protected virtual void SetAccessToken(DaprHttpClientHandler handler, string accessToken)
        {
            handler.PreConfigure(request =>
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            });
        }

        private IdentityClientConfiguration GetClientConfiguration(string identityClientName = null)
        {
            if (identityClientName.IsNullOrEmpty())
            {
                return ClientOptions.IdentityClients.Default;
            }

            return ClientOptions.IdentityClients.GetOrDefault(identityClientName) ??
                   ClientOptions.IdentityClients.Default;
        }
    }
}
