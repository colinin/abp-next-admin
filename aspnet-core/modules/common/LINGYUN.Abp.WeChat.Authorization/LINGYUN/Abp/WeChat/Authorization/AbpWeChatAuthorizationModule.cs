using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using Volo.Abp.Caching;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WeChat.Authorization
{
    [DependsOn(typeof(AbpJsonModule), typeof(AbpCachingModule))]
    public class AbpWeChatAuthorizationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<AbpWeChatOptions>(configuration.GetSection("WeChat:Auth"));

            context.Services.AddHttpClient("WeChatTokenProviderClient", options =>
            {
                options.BaseAddress = new Uri("https://api.weixin.qq.com/cgi-bin/token");
            }).AddTransientHttpErrorPolicy(builder =>
                builder.WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Pow(2, i))));
        }
    }
}
