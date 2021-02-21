using LINGYUN.Abp.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Aliyun
{
    [DependsOn(
        typeof(AbpAliyunModule),
        typeof(AbpTestsBaseModule))]
    public class AbpAliyunTestModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configurationOptions = new AbpConfigurationBuilderOptions
            {
                BasePath = @"D:\Projects\Development\Abp\Aliyun",
                EnvironmentName = "Development"
            };

            context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(configurationOptions));
        }
    }
}
