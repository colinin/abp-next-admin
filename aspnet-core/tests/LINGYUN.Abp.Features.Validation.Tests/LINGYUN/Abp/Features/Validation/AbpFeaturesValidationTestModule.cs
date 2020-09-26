using LINGYUN.Abp.Tests;
using LINGYUN.Abp.Tests.Features;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Features.Validation
{
    [DependsOn(
        typeof(AbpTestsBaseModule),
        typeof(AbpFeaturesValidationModule))]
    public class AbpFeaturesValidationTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<FakeFeatureOptions>(options =>
            {
                options.Map(TestFeatureNames.TestFeature1, (feature) =>
                {
                    return 2.ToString();
                });
            });
        }
    }
}
