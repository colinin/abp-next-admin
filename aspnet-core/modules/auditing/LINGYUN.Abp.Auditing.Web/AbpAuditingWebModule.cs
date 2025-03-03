using LINGYUN.Abp.Auditing.Web.ProfileManagement;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Account.Web.ProfileManagement;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AuditLogging.Localization;
using Volo.Abp.AutoMapper;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.Auditing.Web;

[DependsOn(
    typeof(AbpAccountWebModule),
    typeof(AbpAuditingApplicationContractsModule))]
public class AbpAuditingWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(AuditLoggingResource), typeof(AbpAuditingWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpAuditingWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpAuditingWebModule>();
        });

        ConfigureProfileManagementPage();

        context.Services.AddAutoMapperObjectMapper<AbpAuditingWebModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AbpAuditingWebModule>(validate: true);
        });

        Configure<DynamicJavaScriptProxyOptions>(options =>
        {
            options.DisableModule(AuditingRemoteServiceConsts.ModuleName);
        });
    }

    private void ConfigureProfileManagementPage()
    {
        Configure<ProfileManagementPageOptions>(options =>
        {
            options.Contributors.Add(new SecurityLogManagementPageContributor());
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options.ScriptBundles
                .Configure(typeof(ManageModel).FullName,
                    configuration =>
                    {
                        configuration.AddFiles("/client-proxies/auditing-proxy.js");
                        configuration.AddFiles("/Pages/Account/Components/ProfileManagementGroup/SecurityLog/Index.js");
                    });
        });
    }
}
