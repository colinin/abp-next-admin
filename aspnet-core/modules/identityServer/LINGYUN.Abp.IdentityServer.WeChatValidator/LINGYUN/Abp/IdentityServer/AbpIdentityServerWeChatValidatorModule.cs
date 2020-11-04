using LINGYUN.Abp.IdentityServer.AspNetIdentity;
using LINGYUN.Abp.IdentityServer.WeChatValidator;
using LINGYUN.Abp.WeChat.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.IdentityServer;
using Volo.Abp.IdentityServer.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.IdentityServer
{
    [DependsOn(
        typeof(AbpWeChatAuthorizationModule),
        typeof(AbpIdentityServerDomainModule))]
    public class AbpIdentityServerWeChatValidatorModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            PreConfigure<IIdentityServerBuilder>(builder =>
            {
                builder.AddProfileService<AbpWeChatProfileServicee>();
                builder.AddExtensionGrantValidator<WeChatTokenGrantValidator>();
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<WeChatSignatureOptions>(configuration.GetSection("WeChat:Signature"));

            context.Services
                .AddAuthentication()
                .AddWeChat(options => // 加入微信认证登录
                {
                    configuration.GetSection("WeChat:Auth")?.Bind(options);
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
