using LINGYUN.Abp.IdentityServer.WeChatValidator;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.IdentityServer;
using Volo.Abp.IdentityServer.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.IdentityServer
{
    [DependsOn(typeof(AbpIdentityServerDomainModule))]
    public class AbpIdentityServerWeChatValidatorModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IIdentityServerBuilder>(builder =>
            {
                builder.AddExtensionGrantValidator<WeChatTokenGrantValidator>();
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<AbpWeChatValidatorOptions>(configuration.GetSection("AuthServer:WeChat"));
            Configure<WeChatSignatureOptions>(configuration.GetSection("WeChat:Signature"));

            context.Services.AddHttpClient(WeChatValidatorConsts.WeChatValidatorClientName, options =>
            {
                options.BaseAddress = new System.Uri("https://api.weixin.qq.com/sns/jscode2session");
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpIdentityServerWeChatValidatorModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AbpIdentityServerResource>()
                    .AddVirtualJson("/LINGYUN/Abp/IdentityServer/Localization/WeChatValidator");
            });
        }
    }
}
