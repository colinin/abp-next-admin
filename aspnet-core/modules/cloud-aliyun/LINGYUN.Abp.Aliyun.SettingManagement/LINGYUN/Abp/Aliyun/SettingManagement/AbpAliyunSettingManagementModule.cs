using LINGYUN.Abp.Aliyun.Localization;
using LINGYUN.Abp.Sms.Aliyun;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Aliyun.SettingManagement
{
    [DependsOn(
        typeof(AbpAliyunModule),
        typeof(AbpAliyunSmsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class AbpAliyunSettingManagementModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpAliyunSettingManagementModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAliyunSettingManagementModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AliyunResource>()
                    .AddBaseTypes(typeof(AbpUiResource))
                    .AddVirtualJson("/LINGYUN/Abp/Aliyun/SettingManagement/Localization/Resources");
            });
        }
    }
}
