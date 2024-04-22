using LINGYUN.Abp.WxPusher.Localization;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.WxPusher.SettingManagement
{
    [DependsOn(
        typeof(AbpWxPusherModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class AbpWxPusherSettingManagementModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpWxPusherSettingManagementModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpWxPusherSettingManagementModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<WxPusherResource>()
                    .AddVirtualJson("/LINGYUN/Abp/WxPusher/SettingManagement/Localization/Resources");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<WxPusherResource>()
                    .AddBaseTypes(
                        typeof(AbpUiResource)
                    );
            });
        }
    }
}
