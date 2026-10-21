using LINGYUN.Abp.OpenObserve.Models;
using LINGYUN.Abp.OpenObserve.Models.Report;
using LINGYUN.Abp.OpenObserve.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OpenObserve;

/// <summary>
/// 报表接口
/// </summary>
/// <remarks>
/// 报表接口使用 /api/v2 前缀
/// 参考: https://openobserve.ai/docs/reference/api/report/
/// </remarks>
public partial class OpenObserveClient
{
    /// <summary>
    /// 列出报表
    /// </summary>
    /// <remarks>
    /// GET /api/v2/{org_id}/reports
    /// 响应为裸数组, 无分页包装
    /// </remarks>
    /// <param name="folder">按文件夹过滤, 省略时返回多个文件夹的报表</param>
    /// <param name="dashboardId">仅返回指向该仪表盘的报表</param>
    /// <param name="cache">为 true 时仅返回无投递目标(仅缓存)的报表</param>
    public async virtual Task<ReportInfo[]> GetReportsAsync(
        string? organization = null,
        string? folder = null,
        string? dashboardId = null,
        bool? cache = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = OpenObserveQueryString.Build(
            ("folder", folder),
            ("dashboard_id", dashboardId),
            ("cache", cache));

        var requestUri = $"/api/v2/{ResolveOrganization(organization)}/reports{queryString}";

        return (await GetAsync<ReportInfo[]>(requestUri, cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 创建报表
    /// </summary>
    /// <remarks>
    /// POST /api/v2/{org_id}/reports
    /// </remarks>
    /// <param name="name">报表名称(组织内唯一)</param>
    /// <param name="frequency">调度频率</param>
    /// <param name="dashboards">关联的仪表盘(必填)</param>
    /// <param name="destinations">投递目标(必填)</param>
    /// <param name="startTime">生成起始时间, 转换为微秒提交</param>
    /// <param name="folder">目标文件夹, Enterprise 版必填</param>
    public async virtual Task<OpenObserveCodeMessageResponse> CreateReportAsync(
        string name,
        ReportFrequency frequency,
        ReportDashboardReference[] dashboards,
        ReportDestination[] destinations,
        string? title = null,
        string? description = null,
        string? message = null,
        bool enabled = true,
        DateTime? startTime = null,
        string? timezone = null,
        int? timezoneOffset = null,
        string? folder = null,
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = OpenObserveQueryString.Build(("folder", folder));

        var requestUri = $"/api/v2/{ResolveOrganization(organization)}/reports{queryString}";

        return (await PostJsonAsync<SaveReportRequest, OpenObserveCodeMessageResponse>(
            requestUri,
            CreateReportRequest(name, frequency, dashboards, destinations, title, description,
                message, enabled, startTime, timezone, timezoneOffset, organization),
            cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 获取报表
    /// </summary>
    /// <remarks>
    /// GET /api/v2/{org_id}/reports/{report_id}
    /// </remarks>
    public async virtual Task<ReportInfo> GetReportAsync(
        string reportId,
        string? organization = null,
        string? folder = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = OpenObserveQueryString.Build(("folder", folder));

        var requestUri = $"/api/v2/{ResolveOrganization(organization)}/reports/{Uri.EscapeDataString(reportId)}{queryString}";

        return (await GetAsync<ReportInfo>(requestUri, cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 更新报表
    /// </summary>
    /// <remarks>
    /// PUT /api/v2/{org_id}/reports/{report_id}
    /// 需要提供完整的报表内容
    /// </remarks>
    /// <param name="folder">传入时同时移动报表到该文件夹</param>
    public async virtual Task<OpenObserveCodeMessageResponse> UpdateReportAsync(
        string reportId,
        string name,
        ReportFrequency frequency,
        ReportDashboardReference[] dashboards,
        ReportDestination[] destinations,
        string? title = null,
        string? description = null,
        string? message = null,
        bool enabled = true,
        DateTime? startTime = null,
        string? timezone = null,
        int? timezoneOffset = null,
        string? folder = null,
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = OpenObserveQueryString.Build(("folder", folder));

        var requestUri = $"/api/v2/{ResolveOrganization(organization)}/reports/{Uri.EscapeDataString(reportId)}{queryString}";

        return (await PutJsonAsync<SaveReportRequest, OpenObserveCodeMessageResponse>(
            requestUri,
            CreateReportRequest(name, frequency, dashboards, destinations, title, description,
                message, enabled, startTime, timezone, timezoneOffset, organization),
            cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 删除报表
    /// </summary>
    /// <remarks>
    /// DELETE /api/v2/{org_id}/reports/{report_id}
    /// </remarks>
    public async virtual Task<OpenObserveCodeMessageResponse> DeleteReportAsync(
        string reportId,
        string? organization = null,
        string? folder = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = OpenObserveQueryString.Build(("folder", folder));

        var requestUri = $"/api/v2/{ResolveOrganization(organization)}/reports/{Uri.EscapeDataString(reportId)}{queryString}";

        return (await DeleteAsync<OpenObserveCodeMessageResponse>(requestUri, cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 批量删除报表
    /// </summary>
    /// <remarks>
    /// DELETE /api/v2/{org_id}/reports/bulk
    /// 删除是幂等的, 不存在的标识也会被报告为成功
    /// </remarks>
    public async virtual Task<BulkDeleteReportsResponse> BulkDeleteReportsAsync(
        IEnumerable<string> ids,
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/v2/{ResolveOrganization(organization)}/reports/bulk";

        return (await SendJsonAsync<BulkDeleteReportsRequest, BulkDeleteReportsResponse>(
            HttpMethod.Delete,
            requestUri,
            new BulkDeleteReportsRequest { Ids = [.. ids] },
            cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 移动报表
    /// </summary>
    /// <remarks>
    /// PATCH /api/v2/{org_id}/reports/move
    /// </remarks>
    public async virtual Task<OpenObserveCodeMessageResponse> MoveReportsAsync(
        IEnumerable<string> reportIds,
        string destinationFolderId,
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/v2/{ResolveOrganization(organization)}/reports/move";

        return (await SendJsonAsync<MoveReportsRequest, OpenObserveCodeMessageResponse>(
            HttpMethod.Patch,
            requestUri,
            new MoveReportsRequest
            {
                ReportIds = [.. reportIds],
                DestinationFolderId = destinationFolderId,
            },
            cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 启用/禁用报表
    /// </summary>
    /// <remarks>
    /// PATCH /api/v2/{org_id}/reports/{report_id}/enable?value=true
    /// </remarks>
    public async virtual Task<EnableReportResponse> EnableReportAsync(
        string reportId,
        bool value = true,
        string? organization = null,
        string? folder = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = OpenObserveQueryString.Build(
            ("folder", folder),
            ("value", value));

        var requestUri = $"/api/v2/{ResolveOrganization(organization)}/reports/{Uri.EscapeDataString(reportId)}/enable{queryString}";

        return (await SendJsonAsync<object, EnableReportResponse>(
            HttpMethod.Patch,
            requestUri,
            null,
            cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 手动触发表报
    /// </summary>
    /// <remarks>
    /// PUT /api/v2/{org_id}/reports/{report_id}/trigger
    /// </remarks>
    public async virtual Task<OpenObserveCodeMessageResponse> TriggerReportAsync(
        string reportId,
        string? organization = null,
        string? folder = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = OpenObserveQueryString.Build(("folder", folder));

        var requestUri = $"/api/v2/{ResolveOrganization(organization)}/reports/{Uri.EscapeDataString(reportId)}/trigger{queryString}";

        return (await SendJsonAsync<object, OpenObserveCodeMessageResponse>(
            HttpMethod.Put,
            requestUri,
            null,
            cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 组装报表请求
    /// </summary>
    protected virtual SaveReportRequest CreateReportRequest(
        string name,
        ReportFrequency frequency,
        ReportDashboardReference[] dashboards,
        ReportDestination[] destinations,
        string? title,
        string? description,
        string? message,
        bool enabled,
        DateTime? startTime,
        string? timezone,
        int? timezoneOffset,
        string? organization)
    {
        return new SaveReportRequest
        {
            Name = name,
            Title = title,
            Organization = ResolveOrganization(organization),
            Description = description,
            Message = message,
            Enabled = enabled,
            Frequency = frequency,
            Start = startTime?.ToMicroseconds(),
            Timezone = timezone,
            TimezoneOffset = timezoneOffset,
            Dashboards = dashboards,
            Destinations = destinations,
        };
    }
}
