using Elsa.Extensions;
using Elsa.Features.Services;
using Elsa.Studio.Dashboard.Extensions;
using LINGYUN.Abp.ElsaNext.Studio.Blazor;
using LINGYUN.Abp.ElsaNext.Studio.Dashboard.Blazor.Extensions;
using LINGYUN.Abp.ElsaNext.Studio.Dashboard.Blazor.Navigation;
using LINGYUN.Abp.ElsaNext.Studio.Dashboard.Blazor.Services;
using LINGYUN.Abp.ElsaNext.Studio.Translations.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Routing;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.ElsaNext.Studio.Dashboard.Blazor;

[DependsOn(typeof(AbpElsaNextStudioBlazorModule))]
public class AbpElsaNextStudioDashboardBlazorModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IModule>(elsa =>
        {
            elsa.UseWorkflowRuntimeDashboard();
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Configuration.GetSection("Elsa:Studio");

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(AbpElsaNextStudioDashboardBlazorModule).Assembly);
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new ElsaStudioDashboardMenuContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpElsaNextStudioDashboardBlazorModule>("LINGYUN.Abp.ElsaNext.Studio.Dashboard.Blazor");
        });
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ElsaStudioResource>()
                .AddVirtualJson("/Localization/Resources/ElsaStudioDashboardBlazor");
        });

        var elsaNextStudioBlazoeOptions = context.Services.ExecutePreConfiguredActions<AbpElsaNextStudioBlazorOptions>();

        context.Services.AddDashboardModule(elsaNextStudioBlazoeOptions.BackendApiConfig);
        context.Services.AddDashboardWidgets();

        context.Services.AddScoped<ElsaStudioDashboardText>();
    }
}
