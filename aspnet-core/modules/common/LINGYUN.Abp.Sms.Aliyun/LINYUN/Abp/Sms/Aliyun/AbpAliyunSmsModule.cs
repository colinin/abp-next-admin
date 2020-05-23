using LINYUN.Abp.Sms.Aliyun.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Sms;
using Volo.Abp.VirtualFileSystem;

namespace LINYUN.Abp.Sms.Aliyun
{
    [DependsOn(
        typeof(AbpSmsModule), 
        typeof(AbpJsonModule), 
        typeof(AbpLocalizationModule))]
    public class AbpAliyunSmsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<AliyunSmsOptions>(configuration.GetSection("Aliyun:Sms"));

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAliyunSmsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                       .Add<AliyunSmsResource>("en")
                       .AddVirtualJson("/LINYUN/Abp/Sms/Aliyun/Localization/Resources");
            });
        }
    }
}
