using LINGYUN.Abp.Features.LimitValidation;
using LINGYUN.Abp.WeChat.Common.Localization;
using LINGYUN.Abp.WeChat.Work.Common;
using LINGYUN.Abp.WeChat.Work.Localization;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.Caching;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.WeChat.Work;

[DependsOn(
    typeof(AbpCachingModule),
    typeof(AbpExceptionHandlingModule),
    typeof(AbpFeaturesLimitValidationModule),
    typeof(AbpSettingsModule),
    typeof(AbpWeChatWorkCommonModule))]
public class AbpWeChatWorkModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        Configure<WeChatWorkOptions>(configuration.GetSection("WeChat:Work"));

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpWeChatWorkModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<WeChatWorkResource>("zh-Hans")
                .AddBaseTypes(typeof(WeChatCommonResource))
                .AddVirtualJson("/LINGYUN/Abp/WeChat/Work/Localization/Resources");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(WeChatWorkErrorCodes.Namespace, typeof(WeChatWorkResource));
        });

        context.Services.AddHttpClient(AbpWeChatWorkGlobalConsts.ApiClient,
            options =>
            {
                options.BaseAddress = new Uri("https://qyapi.weixin.qq.com");
            });
        context.Services.AddHttpClient(AbpWeChatWorkGlobalConsts.OAuthClient,
            options =>
            {
                options.BaseAddress = new Uri("https://open.weixin.qq.com");
            });
        context.Services.AddHttpClient(AbpWeChatWorkGlobalConsts.LoginClient,
            options =>
            {
                options.BaseAddress = new Uri("https://login.work.weixin.qq.com");
            });
    }
}
