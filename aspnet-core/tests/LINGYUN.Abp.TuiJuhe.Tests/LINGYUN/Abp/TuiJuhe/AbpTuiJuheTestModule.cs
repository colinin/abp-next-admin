using LINGYUN.Abp.Tests;
using LINGYUN.Abp.Tests.Features;
using LINGYUN.Abp.TuiJuhe.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.TuiJuhe;

[DependsOn(
        typeof(AbpTuiJuheModule),
        typeof(AbpTestsBaseModule))]
public class AbpTuiJuheTestModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configurationOptions = new AbpConfigurationBuilderOptions
        {
            BasePath = @"D:\Projects\Development\Abp\TuiJuhe",
            EnvironmentName = "Test"
        };
        var configuration = ConfigurationHelper.BuildConfiguration(configurationOptions);

        context.Services.ReplaceConfiguration(configuration);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<FakeFeatureOptions>(options =>
        {
            options.Map(TuiJuheFeatureNames.Enable, (_) => "true");
            options.Map(TuiJuheFeatureNames.Message.Enable, (_) => "true");
            options.Map(TuiJuheFeatureNames.Message.SendLimit, (_) => "50");
            options.Map(TuiJuheFeatureNames.Message.SendLimitInterval, (_) => "1");
        });
    }
}
