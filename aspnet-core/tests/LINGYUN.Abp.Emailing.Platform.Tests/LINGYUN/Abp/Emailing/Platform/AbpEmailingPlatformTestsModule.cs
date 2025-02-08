using LINGYUN.Abp.Tests;
using LINGYUN.Platform.HttpApi.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Emailing.Platform;

[DependsOn(
    typeof(PlatformHttpApiClientModule),
    typeof(AbpEmailingPlatformModule),
    typeof(AbpTestsBaseModule))]
public class AbpEmailingPlatformTestsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configurationOptions = new AbpConfigurationBuilderOptions
        {
            BasePath = @"D:\Projects\Development\Abp\Emailing\Platform",
            EnvironmentName = "Test"
        };

        var configuration = ConfigurationHelper.BuildConfiguration(configurationOptions);

        context.Services.ReplaceConfiguration(configuration);
    }
}
