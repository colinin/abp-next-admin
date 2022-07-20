using Elsa.Server.Api;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Elsa;

[DependsOn(
    typeof(AbpElsaModule),
    typeof(AbpAspNetCoreMvcModule))]
public class AbpElsaServerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var preElsaApiOptions = context.Services.GetPreConfigureActions<ElsaApiOptions>();

        context.Services.AddElsaApiEndpoints(options =>
        {
            preElsaApiOptions.Configure(options);
        });
    }
}
