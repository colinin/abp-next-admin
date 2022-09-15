using LINGYUN.Abp.PushPlus.Features;
using LINGYUN.Abp.Tests;
using LINGYUN.Abp.Tests.Features;
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

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<FakeFeatureOptions>(options =>
        {
            options.Map(PushPlusFeatureNames.Message.Enable, (_) => "true");

            options.Map(PushPlusFeatureNames.Channel.Email.Enable, (_) => "true");
            options.Map(PushPlusFeatureNames.Channel.Email.SendLimit, (_) => "500");
            options.Map(PushPlusFeatureNames.Channel.Email.SendLimitInterval, (_) => "1");

            options.Map(PushPlusFeatureNames.Channel.WeChat.Enable, (_) => "true");
            options.Map(PushPlusFeatureNames.Channel.WeChat.SendLimit, (_) => "500");
            options.Map(PushPlusFeatureNames.Channel.WeChat.SendLimitInterval, (_) => "1");
        });
    }
}
