using LINGYUN.Abp.Account.Web.OpenIddict.ViewModels.Authorize;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
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
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AbpOpenIddictResource>()
                .AddVirtualJson("/Localization/Resources");
        });
    }
}
