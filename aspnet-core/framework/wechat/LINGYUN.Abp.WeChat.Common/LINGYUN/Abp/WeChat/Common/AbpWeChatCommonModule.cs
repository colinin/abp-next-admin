using LINGYUN.Abp.WeChat.Common.Localization;
using Volo.Abp.EventBus;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.WeChat.Common;

[DependsOn(
    typeof(AbpEventBusModule))]
public class AbpWeChatCommonModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpWeChatCommonModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<WeChatCommonResource>("zh-Hans")
                .AddVirtualJson("/LINGYUN/Abp/WeChat/Common/Localization/Resources");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(WeChatCommonErrorCodes.Namespace, typeof(WeChatCommonResource));
        });
    }
}
