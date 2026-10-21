using LINGYUN.Abp.OpenObserve.Models.Dashboard;
using LINGYUN.Abp.OpenObserve.Utils;
using System;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OpenObserve;

public partial class OpenObserveClient
{
    /// <summary>
    /// 新增仪表盘面板
    /// </summary>
    /// <remarks>
    /// POST /api/{org_id}/dashboards/{dashboard_id}/panels
    /// 仅支持 v8 仪表盘; hash 为必填参数, 不匹配时返回 409
    /// 参考: https://openobserve.ai/docs/reference/api/dashboard/
    /// </remarks>
    /// <param name="panel">面板定义(v8 面板对象), 新增时可省略 layout 由服务端自动计算</param>
    /// <param name="hash">当前仪表盘哈希</param>
    /// <param name="tabId">页签标识, 默认第一个页签</param>
    /// <param name="folder">仪表盘所在文件夹, 默认文件夹</param>
    public async virtual Task<DashboardPanelResponse> AddDashboardPanelAsync(
        string dashboardId,
        string hash,
        JsonNode panel,
        string? tabId = null,
        string? folder = null,
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = OpenObserveQueryString.Build(
            ("hash", hash),
            ("folder", folder));

        var requestUri = $"/api/{ResolveOrganization(organization)}/dashboards/{Uri.EscapeDataString(dashboardId)}/panels{queryString}";

        var request = new SaveDashboardPanelRequest
        {
            Panel = panel,
            TabId = tabId,
        };

        return (await PostJsonAsync<SaveDashboardPanelRequest, DashboardPanelResponse>(
            requestUri,
            request,
            cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 更新仪表盘面板
    /// </summary>
    /// <remarks>
    /// PUT /api/{org_id}/dashboards/{dashboard_id}/panels/{panel_id}
    /// 路径中的 panel_id 为准; panel 省略 layout 时保留原布局
    /// 参考: https://openobserve.ai/docs/reference/api/dashboard/
    /// </remarks>
    /// <param name="panel">面板定义(v8 面板对象)</param>
    /// <param name="hash">当前仪表盘哈希</param>
    /// <param name="tabId">页签标识, 默认第一个页签</param>
    /// <param name="folder">仪表盘所在文件夹, 默认文件夹</param>
    public async virtual Task<DashboardPanelResponse> UpdateDashboardPanelAsync(
        string dashboardId,
        string panelId,
        string hash,
        JsonNode panel,
        string? tabId = null,
        string? folder = null,
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = OpenObserveQueryString.Build(
            ("hash", hash),
            ("folder", folder));

        var requestUri = $"/api/{ResolveOrganization(organization)}/dashboards/{Uri.EscapeDataString(dashboardId)}/panels/{Uri.EscapeDataString(panelId)}{queryString}";

        var request = new SaveDashboardPanelRequest
        {
            Panel = panel,
            TabId = tabId,
        };

        return (await PutJsonAsync<SaveDashboardPanelRequest, DashboardPanelResponse>(
            requestUri,
            request,
            cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 删除仪表盘面板
    /// </summary>
    /// <remarks>
    /// DELETE /api/{org_id}/dashboards/{dashboard_id}/panels/{panel_id}
    /// 参考: https://openobserve.ai/docs/reference/api/dashboard/
    /// </remarks>
    /// <param name="hash">当前仪表盘哈希</param>
    /// <param name="tabId">指定页签, 省略时在所有页签中查找</param>
    /// <param name="folder">仪表盘所在文件夹, 默认文件夹</param>
    public async virtual Task<DeleteDashboardPanelResponse> DeleteDashboardPanelAsync(
        string dashboardId,
        string panelId,
        string hash,
        string? tabId = null,
        string? folder = null,
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = OpenObserveQueryString.Build(
            ("hash", hash),
            ("tabId", tabId),
            ("folder", folder));

        var requestUri = $"/api/{ResolveOrganization(organization)}/dashboards/{Uri.EscapeDataString(dashboardId)}/panels/{Uri.EscapeDataString(panelId)}{queryString}";

        return (await DeleteAsync<DeleteDashboardPanelResponse>(requestUri, cancellationToken: cancellationToken))!;
    }
}
