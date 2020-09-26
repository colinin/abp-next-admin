using LINGYUN.Abp.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Features.Validation.Redis
{
    [DependsOn(
        typeof(AbpFeaturesValidationTestModule),
        typeof(AbpFeaturesValidationRedisModule),
        typeof(AbpTestsBaseModule))]
    public class AbpFeaturesValidationRedisTestModule : AbpModule
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
