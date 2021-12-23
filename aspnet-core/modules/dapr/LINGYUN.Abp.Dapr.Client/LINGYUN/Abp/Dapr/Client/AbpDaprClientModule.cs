using LINGYUN.Abp.Dapr.Client.DynamicProxying;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Dapr.Client
{
    [DependsOn(
        typeof(AbpHttpClientModule)
        )]
    public class AbpDaprClientModule : AbpModule
    {
        /// <summary>
        /// 与AbpHttpClient集成,创建一个命名HttpClient
        /// </summary>
        internal const string DaprHttpClient = "_AbpDaprProxyClient";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClient(DaprHttpClient);

            context.Services.AddTransient(typeof(DynamicDaprProxyInterceptorClientProxy<>));
        }
    }
}
