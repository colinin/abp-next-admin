using Elsa.Studio.Dashboard.Widgets;
using Elsa.Studio.Diagnostics.OpenTelemetry.Dashboard.UI.Dashboard;
using Microsoft.Extensions.DependencyInjection;

namespace LINGYUN.Abp.ElsaNext.Studio.Diagnostics.OpenTelemetry.Blazor.Extensions;

public static class OpenTelemetryWidgetServiceCollectionExtensions
{
    public static IServiceCollection AddOpenTelemetryDashboardWidget(this IServiceCollection services)
    {
        return services
        .AddDashboardWidget<OpenTelemetryDashboardWidget>(
            "diagnostics.open-telemetry", 
            DashboardWidgetZones.DiagnosticsStatus, 
            300, 
            "OpenTelemetry", 
            requiredBackendCapability: "OpenTelemetry", 
            payloadKind: "OpenTelemetry.StorageDiagnostics");
    }
}
