using LINGYUN.Abp.WeChat.Common;
using LINGYUN.Abp.WeChat.Common.Localization;
using LINGYUN.Abp.WeChat.Localization;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.Caching;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.WeChat;

[DependsOn(
    typeof(AbpCachingModule),
    typeof(AbpFeaturesModule),
    typeof(AbpSettingsModule),
    typeof(AbpWeChatCommonModule))]
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
                .AddBaseTypes(typeof(WeChatCommonResource))
                .AddVirtualJson("/LINGYUN/Abp/WeChat/Localization/Resources");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(WeChatErrorCodes.Namespace, typeof(WeChatResource));
        });

        context.Services.AddHttpClient(AbpWeChatGlobalConsts.HttpClient,
            options =>
            {
                options.BaseAddress = new Uri("https://api.weixin.qq.com");
            });
    }
}
