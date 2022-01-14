using LINGYUN.Abp.WeChat.MiniProgram;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.WeChat.MiniProgram
{
    [DependsOn(
        typeof(AbpWeChatMiniProgramModule), 
        typeof(AbpNotificationModule))]
    public class AbpNotificationsWeChatMiniProgramModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var preActions = context.Services.GetPreConfigureActions<AbpNotificationsWeChatMiniProgramOptions>();

            Configure<AbpNotificationsWeChatMiniProgramOptions>(options =>
            {
                preActions.Configure(options);
            });

            Configure<AbpNotificationOptions>(options =>
            {
                options.PublishProviders.Add<WeChatMiniProgramNotificationPublishProvider>();

                var wechatOptions = preActions.Configure();
                options.NotificationDataMappings
                       .MappingDefault(WeChatMiniProgramNotificationPublishProvider.ProviderName,
                       data => NotificationData.ToStandardData(wechatOptions.DefaultMsgPrefix, data));
            });
        }
    }
}
