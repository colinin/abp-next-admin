using LINGYUN.Abp.OpenObserve.Models;
using LINGYUN.Abp.OpenObserve.Models.Search;
using LINGYUN.Abp.OpenObserve.Utils;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OpenObserve;

public partial class OpenObserveClient
{
    /// <summary>
    /// 搜索
    /// </summary>
    /// <remarks>
    /// POST /api/{organization}/_search
    /// 参考: https://openobserve.ai/docs/reference/api/search/
    /// </remarks>
    /// <typeparam name="THint">命中数据结构</typeparam>
    /// <param name="organization">未指定时使用 AbpOpenObserveOptions.Organization</param>
    /// <param name="query">查询条件, 时间范围为微秒, 必填</param>
    /// <param name="searchType">搜索来源(ui/dashboards/reports/alerts 等)</param>
    /// <param name="timeout">超时时间(秒), 默认 30</param>
    /// <param name="agentOptions">agent/MCP 客户端选项</param>
    /// <param name="useCache">是否使用结果缓存, 省略时由服务端决定(默认 true)</param>
    /// <param name="clearCache">是否清理结果缓存, 为 true 时强制不使用缓存</param>
    public async virtual Task<SearchResponse<THint>> SearchAsync<THint>(
        string? organization,
        SearchQuery query,
        SearchType? searchType = null,
        int? timeout = OpenObserveConstants.ZO_QUERY_TIMEOUT,
        SearchAgentOptions? agentOptions = null,
        bool? useCache = null,
        bool? clearCache = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = OpenObserveQueryString.Build(
            ("use_cache", useCache),
            ("clear_cache", clearCache));

        var requestUri = $"/api/{ResolveOrganization(organization)}/_search{queryString}";

        var response = await PostJsonAsync<SearchRequest, SearchResponse<THint>>(
            requestUri,
            new SearchRequest(query, searchType, timeout, agentOptions),
            ThrowIfSearchException,
            cancellationToken);

        return response!;
    }

    /// <summary>
    /// 搜索
    /// </summary>
    /// <remarks>
    /// POST /api/{organization}/_search, 使用 AbpOpenObserveOptions.Organization
    /// </remarks>
    public async virtual Task<SearchResponse<THint>> SearchAsync<THint>(
        SearchQuery query,
        SearchType? searchType = null,
        int? timeout = OpenObserveConstants.ZO_QUERY_TIMEOUT,
        SearchAgentOptions? agentOptions = null,
        bool? useCache = null,
        bool? clearCache = null,
        CancellationToken cancellationToken = default)
    {
        return await SearchAsync<THint>(
            null,
            query,
            searchType,
            timeout,
            agentOptions,
            useCache,
            clearCache,
            cancellationToken);
    }

    protected virtual Func<Models.OpenObserveErrorResponse, HttpResponseMessage, Exception> ThrowIfSearchException
    {
        get
        {
            return (errorResponse, httpResponse) => new OpenObserveSearchException(
                errorResponse,
                (int)httpResponse.StatusCode);
        }
    }
}
