using LINGYUN.Abp.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Features.LimitValidation.Redis
{
    [DependsOn(
        typeof(AbpFeaturesLimitValidationTestModule),
        typeof(AbpFeaturesValidationRedisModule),
        typeof(AbpTestsBaseModule))]
    public class AbpFeaturesLimitValidationRedisTestModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configurationOptions = new AbpConfigurationBuilderOptions
            {
                BasePath = @"D:\Projects\Development\Abp\FeaturesValidation\Redis",
                EnvironmentName = "Development"
            };

            context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(configurationOptions));
        }
    }
}
