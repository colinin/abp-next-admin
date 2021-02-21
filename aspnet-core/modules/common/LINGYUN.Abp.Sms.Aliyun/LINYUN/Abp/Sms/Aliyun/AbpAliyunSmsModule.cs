using LINGYUN.Abp.Aliyun;
using LINYUN.Abp.Aliyun.Localization;
using LINYUN.Abp.Sms.Aliyun.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Sms;
using Volo.Abp.VirtualFileSystem;

namespace LINYUN.Abp.Sms.Aliyun
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
                       .Add<AliyunSmsResource>("en")
                       .AddBaseTypes(typeof(AliyunResource))
                       .AddVirtualJson("/LINYUN/Abp/Sms/Aliyun/Localization/Resources");
            });
        }
    }
}
