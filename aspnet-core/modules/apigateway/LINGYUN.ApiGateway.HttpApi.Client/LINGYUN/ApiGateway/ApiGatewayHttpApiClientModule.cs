using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace LINGYUN.ApiGateway
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(ApiGatewayApplicationContractsModule)
        )]
    public class ApiGatewayHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "ApiGateway";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(ApiGatewayApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
