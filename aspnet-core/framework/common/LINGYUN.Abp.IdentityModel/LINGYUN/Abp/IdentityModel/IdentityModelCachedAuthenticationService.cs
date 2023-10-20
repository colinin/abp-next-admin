using IdentityModel.Client;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IdentityModel;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Encryption;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.IdentityModel
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    [ExposeServices(
        typeof(IIdentityModelAuthenticationService), 
        typeof(IdentityModelAuthenticationService))]
    public class IdentityModelCachedAuthenticationService : IdentityModelAuthenticationService
    {
        protected IDistributedCache<IdentityModelAuthenticationCacheItem> Cache { get; }
        public IdentityModelCachedAuthenticationService(
            IOptions<AbpIdentityClientOptions> options, 
            ICancellationTokenProvider cancellationTokenProvider, 
            IHttpClientFactory httpClientFactory, 
            ICurrentTenant currentTenant,
            IDistributedCache<IdentityModelAuthenticationCacheItem> cache,
            IOptions<IdentityModelHttpRequestMessageOptions> identityModelHttpRequestMessageOptions) 
            : base(options, cancellationTokenProvider, httpClientFactory, currentTenant, identityModelHttpRequestMessageOptions)
        {
            Cache = cache;
        }

        public override async Task<string> GetAccessTokenAsync(IdentityClientConfiguration configuration)
        {
            var accessTokenCacheItem = await GetCacheItemAsync(configuration);
            // 需要解密
            return accessTokenCacheItem.AccessToken;
        }

        protected async virtual Task<IdentityModelAuthenticationCacheItem> GetCacheItemAsync(IdentityClientConfiguration configuration)
        {
            var cacheKey = IdentityModelAuthenticationCacheItem.CalculateCacheKey(configuration.GrantType, configuration.ClientId, configuration.UserName);
            
            Logger.LogDebug($"IdentityModelCachedAuthenticationService.GetCacheItemAsync: {cacheKey}");

            var cacheItem = await Cache.GetAsync(cacheKey);

            if (cacheItem != null)
            {
                Logger.LogDebug($"Found in the cache: {cacheKey}");
                return cacheItem;
            }

            Logger.LogDebug($"Not found in the cache: {cacheKey}");

            var tokenResponse = await GetAccessTokenResponseAsync(configuration);
            cacheItem = new IdentityModelAuthenticationCacheItem(tokenResponse.AccessToken);
            var cacheEntryOptions = new DistributedCacheEntryOptions
            {
                // 缓存前两分钟过期
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(tokenResponse.ExpiresIn - 120)
            };

            Logger.LogDebug($"Setting the cache item: {cacheKey}");
            await Cache.SetAsync(cacheKey, cacheItem, cacheEntryOptions);

            Logger.LogDebug($"Finished setting the cache item: {cacheKey}");

            return cacheItem;
        }

        protected async virtual Task<TokenResponse> GetAccessTokenResponseAsync(IdentityClientConfiguration configuration)
        {
            var discoveryResponse = await GetDiscoveryResponse(configuration);
            if (discoveryResponse.IsError)
            {
                throw new AbpException($"Could not retrieve the OpenId Connect discovery document! ErrorType: {discoveryResponse.ErrorType}. Error: {discoveryResponse.Error}");
            }

            var tokenResponse = await GetTokenResponse(discoveryResponse, configuration);

            if (tokenResponse.IsError)
            {
                if (tokenResponse.ErrorDescription != null)
                {
                    throw new AbpException($"Could not get token from the OpenId Connect server! ErrorType: {tokenResponse.ErrorType}. Error: {tokenResponse.Error}. ErrorDescription: {tokenResponse.ErrorDescription}. HttpStatusCode: {tokenResponse.HttpStatusCode}");
                }

                var rawError = tokenResponse.Raw;
                var withoutInnerException = rawError.Split(new string[] { "<eof/>" }, StringSplitOptions.RemoveEmptyEntries);
                throw new AbpException(withoutInnerException[0]);
            }

            return tokenResponse;
        }
    }
}
