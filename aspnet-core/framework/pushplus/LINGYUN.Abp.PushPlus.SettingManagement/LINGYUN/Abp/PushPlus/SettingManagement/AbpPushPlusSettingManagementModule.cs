using LINGYUN.Abp.PushPlus.Localization;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.PushPlus.SettingManagement
{
    [DependsOn(
        typeof(AbpPushPlusModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class AbpPushPlusSettingManagementModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpPushPlusSettingManagementModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpPushPlusSettingManagementModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<PushPlusResource>()
                    .AddVirtualJson("/LINGYUN/Abp/PushPlus/SettingManagement/Localization/Resources");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<PushPlusResource>()
                    .AddBaseTypes(
                        typeof(AbpUiResource)
                    );
            });
        }
    }
}
