using LINGYUN.Abp.Aliyun;
using LINGYUN.Abp.Aliyun.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Sms;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Sms.Aliyun
{
    [DependsOn(
        typeof(AbpSmsModule),
        typeof(AbpAliyunModule))]
    public class AbpAliyunSmsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAliyunSmsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                       .Get<AliyunResource>()
                       .AddVirtualJson("/LINGYUN/Abp/Sms/Aliyun/Localization/Resources");
            });
        }
    }
}
