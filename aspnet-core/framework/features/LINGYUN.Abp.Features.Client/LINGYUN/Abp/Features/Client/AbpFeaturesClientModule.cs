using Volo.Abp.Features;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Features.Client
{
    [DependsOn(
        typeof(AbpFeaturesModule))]
    public class AbpFeaturesClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpFeatureOptions>(options =>
            {
                options.ValueProviders.Add<ClientFeatureValueProvider>();
            });
        }
    }
}
