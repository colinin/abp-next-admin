using LINGYUN.Abp.Account.Web.OpenIddict.Pages.Account;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.Localization;
using Volo.Abp.VirtualFileSystem;
using VoloAbpAccountWebOpenIddictModule = Volo.Abp.Account.Web.AbpAccountWebOpenIddictModule;

namespace LINGYUN.Abp.Account.Web.OpenIddict;

[DependsOn(
    typeof(AbpAccountWebModule),
    typeof(VoloAbpAccountWebOpenIddictModule))]
public class AbpAccountWebOpenIddictModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(AbpOpenIddictResource), typeof(AbpAccountWebOpenIddictModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpAccountWebOpenIddictModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAccountWebOpenIddictModule>();
        });

        Configure<RazorViewEngineOptions>(options =>
        {
            options.ViewLocationFormats.Add("/Views/{1}/{0}.cshtml");
        });

        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/Authorize");
            options.Conventions.AuthorizePage("/Account/SelectAccount");
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpOpenIddictResource>()
                .AddVirtualJson("/Localization/Resources");
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options.ScriptBundles
                .Add(typeof(SelectAccountModel).FullName, bundle =>
                {
                    bundle.AddFiles("/Pages/Account/SelectAccount.js");
                });
            options.StyleBundles
                .Add(typeof(SelectAccountModel).FullName, bundle =>
                {
                    bundle.AddFiles("/css/select-account.css");
                });
        });
    }
}
