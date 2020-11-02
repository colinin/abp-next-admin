using LINGYUN.Abp.WeChat.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.WeChat.WeApp
{
    [DependsOn(
        typeof(AbpWeChatAuthorizationModule), 
        typeof(AbpNotificationModule))]
    public class AbpNotificationsWeChatWeAppModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<AbpWeChatWeAppNotificationOptions>(configuration.GetSection("Notifications:WeChat:WeApp"));

            // TODO:是否有必要启用重试机制?
            context.Services.AddHttpClient(WeChatWeAppNotificationSender.SendNotificationClientName)
                .AddTransientHttpErrorPolicy(builder =>
                    builder.WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Pow(2, i))));

            Configure<AbpNotificationOptions>(options =>
            {
                options.PublishProviders.Add<WeChatWeAppNotificationPublishProvider>();
            });
        }
    }
}
