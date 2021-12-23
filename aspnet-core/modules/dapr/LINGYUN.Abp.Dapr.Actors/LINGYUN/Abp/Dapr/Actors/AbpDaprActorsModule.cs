using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Dapr.Actors
{
    [DependsOn(
        typeof(AbpHttpClientModule)
        )]
    public class AbpDaprActorsModule : AbpModule
    {
        /// <summary>
        /// 与AbpHttpClient集成,创建一个命名HttpClient
        /// </summary>
        internal const string DaprHttpClient = "_AbpDaprActorsClient";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            context.Services.AddHttpClient(DaprHttpClient);
        }
    }
}
