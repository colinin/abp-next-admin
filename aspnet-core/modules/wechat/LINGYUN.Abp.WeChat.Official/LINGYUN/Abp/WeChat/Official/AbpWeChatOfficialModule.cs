using LINGYUN.Abp.WeChat.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.WeChat.Official
{
    [DependsOn(
        typeof(AbpWeChatModule))]
    public class AbpWeChatOfficialModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpWeChatOfficialModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<WeChatResource>()
                    .AddVirtualJson("/LINGYUN/Abp/WeChat/Official/Localization/Resources");
            });

            context.Services.AddAbpDynamicOptions<AbpWeChatOfficialOptions, AbpWeChatOfficialOptionsManager>();
        }
    }
}
