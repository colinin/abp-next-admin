using AspNet.Security.OAuth.Bilibili;
using AspNet.Security.OAuth.GitHub;
using AspNet.Security.OAuth.QQ;
using AspNet.Security.OAuth.Weixin;
using AspNet.Security.OAuth.WorkWeixin;
using LINGYUN.Abp.Account.OAuth;
using LINGYUN.Abp.Account.OAuth.Localization;
using LINGYUN.Abp.Account.Web.OAuth.ExternalProviders.Bilibili;
using LINGYUN.Abp.Account.Web.OAuth.ExternalProviders.GitHub;
using LINGYUN.Abp.Account.Web.OAuth.ExternalProviders.QQ;
using LINGYUN.Abp.Account.Web.OAuth.ExternalProviders.WeChat;
using LINGYUN.Abp.Account.Web.OAuth.ExternalProviders.WeCom;
using LINGYUN.Abp.Account.Web.OAuth.Microsoft.Extensions.DependencyInjection;
using LINGYUN.Abp.Tencent.QQ;
using LINGYUN.Abp.WeChat.Official;
using LINGYUN.Abp.WeChat.Work;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Account.Web.OAuth;

[DependsOn(typeof(AbpAccountWebModule))]
[DependsOn(typeof(AbpAccountOAuthModule))]
[DependsOn(typeof(AbpTencentQQModule))]
[DependsOn(typeof(AbpWeChatOfficialModule))]
[DependsOn(typeof(AbpWeChatWorkModule))]
public class AbpAccountWebOAuthModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(AccountResource), typeof(AbpAccountWebOAuthModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpAccountWebOAuthModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAccountWebOAuthModule>("LINGYUN.Abp.Account.Web.OAuth");
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AccountResource>()
                .AddBaseTypes(typeof(AccountOAuthResource));
        });

        context.Services
            .AddAuthentication()
            .AddGitHub(options =>
            {
                options.ClientId = "ClientId";
                options.ClientSecret = "ClientSecret";

                options.Scope.Add("user:email");
            }).UseSettingProvider<
                GitHubAuthenticationOptions,
                GitHubAuthenticationHandler,
                GitHubAuthHandlerOptionsProvider>()
            .AddQQ(options =>
            {
                options.ClientId = "ClientId";
                options.ClientSecret = "ClientSecret";
            }).UseSettingProvider<
                QQAuthenticationOptions,
                QQAuthenticationHandler,
                QQAuthHandlerOptionsProvider>()
            .AddWeixin(options =>
            {
                options.ClientId = "ClientId";
                options.ClientSecret = "ClientSecret";
            }).UseSettingProvider<
                WeixinAuthenticationOptions,
                WeixinAuthenticationHandler,
                WeChatAuthHandlerOptionsProvider>()
            .AddWorkWeixin(options =>
            {
                options.ClientId = "ClientId";
                options.ClientSecret = "ClientSecret";
            }).UseSettingProvider<
                WorkWeixinAuthenticationOptions,
                WorkWeixinAuthenticationHandler,
                WeComAuthHandlerOptionsProvider>()
            .AddBilibili(options =>
            {
                options.ClientId = "ClientId";
                options.ClientSecret = "ClientSecret";
            }).UseSettingProvider<
                BilibiliAuthenticationOptions,
                BilibiliAuthenticationHandler,
                BilibiliAuthHandlerOptionsProvider>();
    }
}
