using Elsa.Diagnostics.StructuredLogs.Extensions;
using Elsa.Extensions;
using Elsa.Features.Services;
using Elsa.Studio.Diagnostics.StructuredLogs.Dashboard.Extensions;
using Elsa.Studio.Diagnostics.StructuredLogs.Extensions;
using LINGYUN.Abp.ElsaNext.Studio.Blazor;
using LINGYUN.Abp.ElsaNext.Studio.Diagnostics.StructuredLogs.Blazor.Extensions;
using LINGYUN.Abp.ElsaNext.Studio.Diagnostics.StructuredLogs.Blazor.Navigation;
using LINGYUN.Abp.ElsaNext.Studio.Translations.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Routing;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.ElsaNext.Studio.Diagnostics.StructuredLogs.Blazor;

[DependsOn(
    typeof(AbpAspNetCoreModule),
    typeof(AbpElsaNextStudioBlazorModule))]
public class AbpElsaNextStudioDiagnosticsStructuredLogsBlazorModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IModule>(elsa =>
        {
            elsa.UseStructuredLogs();
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Configuration.GetSection("Elsa:Studio");

        Configure<AbpRouterOptions>(options =>
        {
            // options.AdditionalAssemblies.Add(typeof(Elsa.Studio.Diagnostics.StructuredLogs.Feature).Assembly);
            options.AdditionalAssemblies.Add(typeof(AbpElsaNextStudioDiagnosticsStructuredLogsBlazorModule).Assembly);
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new ElsaStudioStructuredLogsMenuContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpElsaNextStudioDiagnosticsStructuredLogsBlazorModule>("LINGYUN.Abp.ElsaNext.Studio.Diagnostics.StructuredLogs.Blazor");
        });
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ElsaStudioResource>()
                .AddVirtualJson("/Localization/Resources/ElsaStudioStructuredLogsBlazor");
        });

        var elsaNextStudioBlazoeOptions = context.Services.ExecutePreConfiguredActions<AbpElsaNextStudioBlazorOptions>();

        context.Services.AddStructuredLogsModule(elsaNextStudioBlazoeOptions.BackendApiConfig);
        context.Services.AddStructuredLogsDashboardModule();
        context.Services.AddStructuredLogsDashboardWidget();

        Configure<AbpEndpointRouterOptions>(options =>
        {
            options.EndpointConfigureActions.Add((context) =>
            {
                context.Endpoints.MapStructuredLogsHub();
            });
        });
    }
}
