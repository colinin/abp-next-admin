using LINGYUN.Abp.WeChat.Localization;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using Volo.Abp.Caching;
using Volo.Abp.Features;
using Volo.Abp.Json;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.WeChat
{
    [DependsOn(
        typeof(AbpCachingModule),
        typeof(AbpFeaturesModule),
        typeof(AbpJsonModule))]
    public class AbpWeChatModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpWeChatModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<WeChatResource>("zh-Hans")
                    .AddVirtualJson("/LINGYUN/Abp/WeChat/Localization/Resources");
            });

            context.Services.AddHttpClient(AbpWeChatGlobalConsts.HttpClient,
                options =>
                {
                    options.BaseAddress = new Uri("https://api.weixin.qq.com");
                }).AddTransientHttpErrorPolicy(builder =>
                    builder.WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Pow(2, i))));
        }
    }
}
