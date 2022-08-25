using LINGYUN.Abp.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.PushPlus;

[DependsOn(
        typeof(AbpPushPlusModule),
        typeof(AbpTestsBaseModule))]
public class AbpPushPlusTestModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configurationOptions = new AbpConfigurationBuilderOptions
        {
            BasePath = @"D:\Projects\Development\Abp\PushPlus",
            EnvironmentName = "Test"
        };
        var configuration = ConfigurationHelper.BuildConfiguration(configurationOptions);

        context.Services.ReplaceConfiguration(configuration);
    }
}
