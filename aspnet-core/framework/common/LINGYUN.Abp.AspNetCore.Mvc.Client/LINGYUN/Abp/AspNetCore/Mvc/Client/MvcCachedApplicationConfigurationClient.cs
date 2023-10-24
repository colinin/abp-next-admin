using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.DynamicProxying;
using Volo.Abp.Threading;
using Volo.Abp.Users;

namespace LINGYUN.Abp.AspNetCore.Mvc.Client
{
    [ExposeServices(
        typeof(MvcCachedApplicationConfigurationClient),
        typeof(ICachedApplicationConfigurationClient)
        )]
    public class MvcCachedApplicationConfigurationClient : ICachedApplicationConfigurationClient, ITransientDependency
    {
        protected IHttpContextAccessor HttpContextAccessor { get; }
        protected IHttpClientProxy<IAbpApplicationConfigurationAppService> Proxy { get; }
        protected ICurrentUser CurrentUser { get; }
        protected IDistributedCache<ApplicationConfigurationDto> Cache { get; }
        protected AbpAspNetCoreMvcClientCacheOptions MvcClientCacheOptions { get; }

        public MvcCachedApplicationConfigurationClient(
            IDistributedCache<ApplicationConfigurationDto> cache,
            IHttpClientProxy<IAbpApplicationConfigurationAppService> proxy,
            ICurrentUser currentUser,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AbpAspNetCoreMvcClientCacheOptions> mvcClientCacheOptions)
        {
            Proxy = proxy;
            CurrentUser = currentUser;
            HttpContextAccessor = httpContextAccessor;
            Cache = cache;
            MvcClientCacheOptions = mvcClientCacheOptions.Value;
        }

        public async Task<ApplicationConfigurationDto> GetAsync()
        {
            var cacheKey = CreateCacheKey();
            var httpContext = HttpContextAccessor?.HttpContext;

            if (httpContext != null && httpContext.Items[cacheKey] is ApplicationConfigurationDto configuration)
            {
                return configuration;
            }

            configuration = await Cache.GetOrAddAsync(
                cacheKey,
                async () => await Proxy.Service.GetAsync(new ApplicationConfigurationRequestOptions()),
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(CurrentUser.IsAuthenticated 
                    ? MvcClientCacheOptions.UserCacheExpirationSeconds 
                    : MvcClientCacheOptions.AnonymousCacheExpirationSeconds)
                }
            );

            if (httpContext != null)
            {
                httpContext.Items[cacheKey] = configuration;
            }

            return configuration;
        }

        public ApplicationConfigurationDto Get()
        {
            var cacheKey = CreateCacheKey();
            var httpContext = HttpContextAccessor?.HttpContext;

            if (httpContext != null && httpContext.Items[cacheKey] is ApplicationConfigurationDto configuration)
            {
                return configuration;
            }

            return AsyncHelper.RunSync(GetAsync);
        }

        protected virtual string CreateCacheKey()
        {
            return MvcCachedApplicationConfigurationClientHelper.CreateCacheKey(CurrentUser);
        }
    }
}
