using LINGYUN.Abp.Account.Emailing;
using LINGYUN.Abp.Account.Web.Bundling;
using LINGYUN.Abp.Account.Web.Pages.Account;
using LINGYUN.Abp.Account.Web.ProfileManagement;
using LINGYUN.Abp.Identity;
using LINGYUN.Abp.Identity.AspNetCore.QrCode;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.Account.Localization;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Account.Web.ProfileManagement;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.QRCode;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Sms;
using Volo.Abp.VirtualFileSystem;
using VoloAbpAccountWebModule = Volo.Abp.Account.Web.AbpAccountWebModule;

namespace LINGYUN.Abp.Account.Web;

[DependsOn(
    typeof(AbpSmsModule),
    typeof(VoloAbpAccountWebModule),
    typeof(AbpIdentityDomainModule),
    typeof(AbpAccountEmailingModule),
    typeof(AbpIdentityAspNetCoreQrCodeModule),
    typeof(AbpAccountApplicationContractsModule))]
public class AbpAccountWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(AccountResource), typeof(AbpAccountWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpAccountWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAccountWebModule>();
        });

        ConfigureProfileManagementPage();

        context.Services.AddAutoMapperObjectMapper<AbpAccountWebModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AbpAccountWebModule>(validate: true);
        });

        context.Services
            .AddAuthentication()
            .AddCookie(AbpAccountAuthenticationTypes.ShouldChangePassword, options =>
            {
                options.LoginPath = new PathString("/Account/Login");
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5.0);
                options.Events = new CookieAuthenticationEvents
                {
                    OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
                };
            });
    }

    private void ConfigureProfileManagementPage()
    {
        Configure<ProfileManagementPageOptions>(options =>
        {
            options.Contributors.Add(new ProfileManagementPageContributor());
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles
                .Add(AccountBundles.Styles.UserLoginLink, bundle =>
                {
                    bundle.AddContributors(typeof(UserLoginLinkStyleContributor));
                });

            options.ScriptBundles
                .Configure(typeof(ManageModel).FullName,
                    configuration =>
                    {
                        // Client Proxies
                        configuration.AddFiles("/client-proxies/account-proxy.js");

                        // Session
                        configuration.AddFiles("/Pages/Account/Components/ProfileManagementGroup/Session/Index.js");

                        // Authenticator
                        configuration.AddFiles("/Pages/Account/Components/ProfileManagementGroup/Authenticator/Index.js");

                        // SecurityLog
                        configuration.AddFiles("/Pages/Account/Components/ProfileManagementGroup/SecurityLog/Index.js");

                        // TwoFactor
                        configuration.AddFiles("/Pages/Account/Components/ProfileManagementGroup/TwoFactor/Default.js");

                        // QrCode
                        configuration.AddContributors(typeof(QRCodeScriptContributor));
                    });
            options.ScriptBundles
                .Configure(AccountBundles.Scripts.ChangePassword, bundle =>
                {
                    bundle.AddContributors(typeof(ChangePasswordScriptContributor));
                });
        });
    }
}
