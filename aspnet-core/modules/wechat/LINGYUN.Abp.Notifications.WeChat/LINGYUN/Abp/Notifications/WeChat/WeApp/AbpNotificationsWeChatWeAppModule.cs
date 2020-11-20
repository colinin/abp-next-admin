using LINGYUN.Abp.WeChat.MiniProgram;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.WeChat.WeApp
{
    [DependsOn(
        typeof(AbpWeChatMiniProgramModule), 
        typeof(AbpNotificationModule))]
    public class AbpNotificationsWeChatWeAppModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<AbpWeChatWeAppNotificationOptions>(configuration.GetSection("Notifications:WeChat:WeApp"));

            Configure<AbpNotificationOptions>(options =>
            {
                options.PublishProviders.Add<WeChatWeAppNotificationPublishProvider>();
            });
        }
    }
}
