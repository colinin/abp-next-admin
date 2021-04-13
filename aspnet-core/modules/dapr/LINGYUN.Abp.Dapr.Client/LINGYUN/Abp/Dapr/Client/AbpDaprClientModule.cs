using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Dapr.Client
{
    [DependsOn(
        typeof(AbpHttpClientModule))]
    public class AbpDaprClientModule : AbpModule
    {
        /// <summary>
        /// 与AbpHttpClient集成,创建一个命名HttpClient
        /// </summary>
        internal const string DaprHttpClient = "_AbpDaprClient";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<AbpDaprRemoteServiceOptions>(configuration);
            Configure<AbpDaprClientOptions>(configuration.GetSection("Dapr:Client"));

            // DaprClient应该配置为单例的实现
            context.Services.AddDaprClient();
        }
    }
}
