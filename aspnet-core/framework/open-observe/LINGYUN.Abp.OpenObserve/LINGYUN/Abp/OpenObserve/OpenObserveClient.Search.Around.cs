using LINGYUN.Abp.OpenObserve.Models.Search;
using LINGYUN.Abp.OpenObserve.Models.Stream;
using LINGYUN.Abp.OpenObserve.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OpenObserve;

public partial class OpenObserveClient
{
    /// <summary>
    /// 查询某条记录前后的数据
    /// </summary>
    /// <remarks>
    /// GET /api/{organization}/{stream}/_around
    /// 以 key(_timestamp) 对应的记录为中心, 前后各 5 分钟范围内查询
    /// 参考: https://openobserve.ai/docs/reference/api/search/
    /// </remarks>
    /// <param name="key">中心记录的 _timestamp(微秒)</param>
    /// <param name="size">返回的条数</param>
    public async virtual Task<SearchResponse<THint>> AroundAsync<THint>(
        string stream,
        long key,
        long? size = null,
        StreamType? type = null,
        int? timeout = null,
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = OpenObserveQueryString.Build(
            ("key", key),
            ("size", size),
            ("type", type?.AsString()),
            ("timeout", timeout));

        var requestUri = $"/api/{ResolveOrganization(organization)}/{Uri.EscapeDataString(stream)}/_around{queryString}";

        return (await GetAsync<SearchResponse<THint>>(requestUri, cancellationToken: cancellationToken))!;
    }
}
