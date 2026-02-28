using LINGYUN.Abp.Gdpr.Localization;
using LINGYUN.Abp.Gdpr.Web.Pages.Account;
using LINGYUN.Abp.Gdpr.Web.ProfileManagement;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Account.Web.ProfileManagement;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Gdpr.Web;

[DependsOn(
    typeof(AbpAccountWebModule),
    typeof(AbpGdprApplicationContractsModule))]
public class AbpGdprWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(GdprResource), typeof(AbpGdprWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpGdprWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpGdprWebModule>();
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new AbpGdprUserMenuContributor());
        });

        ConfigureProfileManagementPage();

        Configure<DynamicJavaScriptProxyOptions>(options =>
        {
            options.DisableModule(GdprRemoteServiceConsts.ModuleName);
        });
    }

    private void ConfigureProfileManagementPage()
    {
        Configure<ProfileManagementPageOptions>(options =>
        {
            options.Contributors.Add(new GdprManagementPageContributor());
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options.ScriptBundles
                .Configure(typeof(ManageModel).FullName,
                    configuration =>
                    {
                        configuration.AddFiles("/client-proxies/gdpr-proxy.js");
                        configuration.AddFiles("/Pages/Account/Components/ProfileManagementGroup/Gdpr/Index.js");
                    });
            options.ScriptBundles
                .Configure(typeof(DeleteModel).FullName,
                    configuration =>
                    {
                        configuration.AddFiles("/client-proxies/gdpr-proxy.js");
                        configuration.AddFiles("/Pages/Account/Delete.js");
                    });
        });
    }
}
