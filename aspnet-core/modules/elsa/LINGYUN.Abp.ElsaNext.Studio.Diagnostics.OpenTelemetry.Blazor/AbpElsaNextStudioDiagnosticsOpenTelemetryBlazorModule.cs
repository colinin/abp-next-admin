using Elsa.Diagnostics.OpenTelemetry.Extensions;
using Elsa.Extensions;
using Elsa.Features.Services;
using Elsa.OpenTelemetry.Middleware;
using Elsa.Studio.Diagnostics.OpenTelemetry.Dashboard.Extensions;
using Elsa.Studio.Diagnostics.OpenTelemetry.Extensions;
using Elsa.Workflows.Telemetry;
using LINGYUN.Abp.ElsaNext.Studio.Blazor;
using LINGYUN.Abp.ElsaNext.Studio.Diagnostics.OpenTelemetry.Blazor.Extensions;
using LINGYUN.Abp.ElsaNext.Studio.Diagnostics.OpenTelemetry.Blazor.Navigation;
using LINGYUN.Abp.ElsaNext.Studio.Translations.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Routing;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.ElsaNext.Studio.Diagnostics.OpenTelemetry.Blazor;

[DependsOn(
    typeof(AbpAspNetCoreModule),
    typeof(AbpElsaNextStudioBlazorModule))]
public class AbpElsaNextStudioDiagnosticsOpenTelemetryBlazorModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IModule>(elsa =>
        {
            elsa.UseWorkflows(workflows =>
            {
                workflows.WithDefaultWorkflowExecutionPipeline(pipeline =>
                    pipeline.UseWorkflowExecutionTracing());
                workflows.WithDefaultActivityExecutionPipeline(pipeline =>
                    pipeline.UseActivityExecutionTracing());
            });
            elsa.UseOpenTelemetry();
            elsa.UseOpenTelemetryDiagnostics();
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Configuration.GetSection("Elsa:Studio");

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(Elsa.Studio.Diagnostics.OpenTelemetry.Feature).Assembly);
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new ElsaStudioOpenTelemetryMenuContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpElsaNextStudioDiagnosticsOpenTelemetryBlazorModule>("LINGYUN.Abp.ElsaNext.Studio.Diagnostics.OpenTelemetry.Blazor");
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ElsaStudioResource>()
                .AddVirtualJson("/Localization/Resources/ElsaStudioOpenTelemetryBlazor");
        });

        var elsaNextStudioBlazoeOptions = context.Services.ExecutePreConfiguredActions<AbpElsaNextStudioBlazorOptions>();

        context.Services.AddOpenTelemetryDiagnosticsModule(elsaNextStudioBlazoeOptions.BackendApiConfig);
        context.Services.AddOpenTelemetryDashboardModule();
        context.Services.AddOpenTelemetryDashboardWidget();
        context.Services.AddOpenTelemetry()
            .WithTracing(tracing => tracing.AddSource(WorkflowInstrumentation.ActivitySourceName))
            .WithMetrics(metrics => metrics.AddMeter(WorkflowInstrumentation.MeterName));

        Configure<AbpEndpointRouterOptions>(options =>
        {
            options.EndpointConfigureActions.Add((context) =>
            {
                context.Endpoints.MapOpenTelemetryHub();
            });
        });
    }
}
