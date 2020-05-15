using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace LINGYUN.ApiGateway
{
    [DependsOn(
        typeof(ApiGatewayDomainModule),
        typeof(ApiGatewayApplicationContractsModule),
        typeof(AbpAutoMapperModule)
        )]
    public class ApiGatewayApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.Configurators.Add(ctx =>
                {
                    var mapperProfile = ctx.ServiceProvider.GetService<ApiGatewayApplicationAutoMapperProfile>();
                    ctx.MapperConfiguration.AddProfile(mapperProfile);
                });
            });
        }
    }
}
