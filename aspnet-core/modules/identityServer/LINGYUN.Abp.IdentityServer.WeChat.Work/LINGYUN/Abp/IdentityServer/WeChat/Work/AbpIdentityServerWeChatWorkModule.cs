using LINGYUN.Abp.WeChat.Work;
using LINGYUN.Abp.WeChat.Work.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Identity.Localization;
using Volo.Abp.IdentityServer;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.IdentityServer.WeChat.Work;

[DependsOn(
    typeof(AbpIdentityServerDomainModule),
    typeof(AbpWeChatWorkModule))]
public class AbpIdentityServerWeChatWorkModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IIdentityServerBuilder>(builder =>
        {
            builder.AddExtensionGrantValidator<WeChatWorkGrantValidator>();
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpIdentityServerWeChatWorkModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<WeChatWorkResource>()
                .AddBaseTypes(typeof(IdentityResource))
                .AddVirtualJson("/LINGYUN/Abp/IdentityServer/WeChat/Work/Localization/Resources");
        });
    }
}
