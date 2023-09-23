using LINGYUN.Abp.Tests;
using LINGYUN.Abp.Tests.Features;
using LINGYUN.Abp.WeChat.Work.Features;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WeChat.Work;

[DependsOn(
    typeof(AbpWeChatWorkModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpTestsBaseModule))]
public class AbpWeChatWorkTestModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configurationOptions = new AbpConfigurationBuilderOptions
        {
            BasePath = @"D:\Projects\Development\Abp\WeChat\Work",
            EnvironmentName = "Test"
        };

        context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(configurationOptions));
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        Configure<AbpDistributedCacheOptions>(options =>
        {
            configuration.GetSection("DistributedCache").Bind(options);
        });

        Configure<RedisCacheOptions>(options =>
        {
            var redisConfig = ConfigurationOptions.Parse(options.Configuration);
            options.ConfigurationOptions = redisConfig;
            options.InstanceName = configuration["Redis:InstanceName"];
        });
        Configure<FakeFeatureOptions>(options =>
        {
            options.Map(WeChatWorkFeatureNames.Enable, (feature) =>
            {
                return true.ToString();
            });
            options.Map(WeChatWorkFeatureNames.Message.Enable, (feature) =>
            {
                return true.ToString();
            });
            options.Map(WeChatWorkFeatureNames.AppChat.Message.Enable, (feature) =>
            {
                return true.ToString();
            });
        });
    }
}
