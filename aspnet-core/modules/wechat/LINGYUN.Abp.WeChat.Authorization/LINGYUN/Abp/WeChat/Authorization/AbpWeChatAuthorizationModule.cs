using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using Volo.Abp.Caching;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WeChat.Authorization
{
    [DependsOn(
        typeof(AbpWeChatModule),
        typeof(AbpJsonModule), 
        typeof(AbpCachingModule))]
    public class AbpWeChatAuthorizationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<AbpWeChatAuthorizationOptions>(configuration.GetSection("WeChat:Auth"));

            context.Services.AddHttpClient("WeChatRequestClient", options =>
            {
                options.BaseAddress = new Uri("https://api.weixin.qq.com");
            }).AddTransientHttpErrorPolicy(builder =>
                builder.WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Pow(2, i))));
        }
    }
}
