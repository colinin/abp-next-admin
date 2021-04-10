using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Castle;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Dapr.Actors
{
    [DependsOn(
        typeof(AbpCastleCoreModule),
        typeof(AbpMultiTenancyModule),
        typeof(AbpValidationModule),
        typeof(AbpExceptionHandlingModule)
        )]
    public class AbpDaprActorsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<AbpDaprActorOptions>(configuration);
        }
    }
}
