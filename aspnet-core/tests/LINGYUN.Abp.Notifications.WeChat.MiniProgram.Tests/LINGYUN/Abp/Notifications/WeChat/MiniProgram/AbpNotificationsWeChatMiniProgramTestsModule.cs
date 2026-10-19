using LINGYUN.Abp.Tests;
using LINGYUN.Abp.WeChat.MiniProgram.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Notifications.WeChat.MiniProgram
{
    [DependsOn(
        typeof(AbpNotificationsWeChatMiniProgramModule),
        typeof(AbpNotificationsTestsModule),
        typeof(AbpTestsBaseModule))]
    public class AbpNotificationsWeChatMiniProgramTestsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AbpNotificationsWeChatMiniProgramOptions>(options =>
            {
                // 默认值, 便于断言"未写入渠道数据时的兜底"
                options.DefaultTemplateId = "DEFAULT_TEMPLATE_ID";
                options.DefaultState = "formal";
                options.DefaultLanguage = "zh_CN";
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // 用捕获式假实现替换真实发送, 避免真正调用微信接口
            context.Services.Replace(
                ServiceDescriptor.Singleton<ISubscribeMessager, FakeSubscribeMessager>());
        }
    }
}
