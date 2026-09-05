using Elsa.Studio.Dashboard.Widgets;
using LINGYUN.Abp.ElsaNext.Studio.Dashboard.Blazor.Widgets;
using Microsoft.Extensions.DependencyInjection;

namespace LINGYUN.Abp.ElsaNext.Studio.Dashboard.Blazor.Extensions;

public static class DashboardWidgetServiceCollectionExtensions
{
    public static IServiceCollection AddDashboardWidgets(this IServiceCollection services)
    {
        return services
        .AddDashboardWidget<OperationalHealthWidget>("dashboard.workflow.metrics", DashboardWidgetZones.Metrics, 100, "Workflow metrics", payloadKind: "WorkflowInstances")
        .AddDashboardWidget<NeedsAttentionWidget>("dashboard.needs-attention", DashboardWidgetZones.Findings, 100, "Needs attention")
        .AddDashboardWidget<ExecutionTrendWidget>("dashboard.workflow.trend", DashboardWidgetZones.Trend, 100, "Workflow trends", payloadKind: "WorkflowTrends")
        .AddDashboardWidget<RecentActivityWidget>("dashboard.workflow.recent-activity", DashboardWidgetZones.Activity, 100, "Recent activity", payloadKind: "RecentActivity")
        .AddDashboardWidget<HotspotsWidget>("dashboard.workflow.hotspots", DashboardWidgetZones.SecondaryPanels, 100, "Workflow hotspots", payloadKind: "WorkflowHotspots");
    }
}
