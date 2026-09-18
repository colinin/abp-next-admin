using Elsa.Studio.Dashboard.Widgets;
using Elsa.Studio.Diagnostics.StructuredLogs.Dashboard.UI.Dashboard;
using Microsoft.Extensions.DependencyInjection;

namespace LINGYUN.Abp.ElsaNext.Studio.Diagnostics.StructuredLogs.Blazor.Extensions;

public static class OpenTelemetryWidgetServiceCollectionExtensions
{
    public static IServiceCollection AddStructuredLogsDashboardWidget(this IServiceCollection services)
    {
        return services
        .AddDashboardWidget<StructuredLogsDashboardWidget>(
            "diagnostics.structured-logs", 
            DashboardWidgetZones.DiagnosticsStatus, 
            100,
            "Structured logs", 
            requiredBackendCapability: "StructuredLogs", 
            payloadKind: "Diagnostics.StructuredLogs");
    }
}
