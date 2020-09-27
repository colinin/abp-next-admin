using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Features.Validation
{
    [DependsOn(typeof(AbpFeaturesModule))]
    public class AbpFeaturesValidationModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.OnRegistred(FeaturesValidationInterceptorRegistrar.RegisterIfNeeded);

            Configure<AbpFeaturesValidationOptions>(options =>
            {
                options.MapDefaultEffectPolicys();
            });
        }
    }
}
