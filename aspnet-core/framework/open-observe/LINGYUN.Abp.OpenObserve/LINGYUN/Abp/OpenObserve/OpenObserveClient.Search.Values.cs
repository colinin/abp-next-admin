using LINGYUN.Abp.OpenObserve.Models.Search;
using LINGYUN.Abp.OpenObserve.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OpenObserve;

public partial class OpenObserveClient
{
    /// <summary>
    /// 查询字段取值
    /// </summary>
    /// <remarks>
    /// GET /api/{organization}/{stream}/_values
    /// 参考: https://openobserve.ai/docs/reference/api/search/
    /// </remarks>
    /// <param name="fields">字段名称集合, 逗号拼接</param>
    /// <param name="size">返回的取值数量, 按出现次数排序</param>
    /// <param name="keyword">取值关键字过滤</param>
    /// <param name="noCount">为 true 时不返回计数, 直接按取值排序</param>
    public async virtual Task<ValuesResponse> GetValuesAsync(
        string stream,
        IEnumerable<string> fields,
        DateTime startTime,
        DateTime endTime,
        long? size = null,
        string? keyword = null,
        bool? noCount = null,
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = OpenObserveQueryString.Build(
            ("fields", string.Join(",", fields)),
            ("start_time", startTime.ToMicroseconds()),
            ("end_time", endTime.ToMicroseconds()),
            ("size", size),
            ("keyword", keyword),
            ("no_count", noCount));

        var requestUri = $"/api/{ResolveOrganization(organization)}/{Uri.EscapeDataString(stream)}/_values{queryString}";

        return (await GetAsync<ValuesResponse>(requestUri, cancellationToken: cancellationToken))!;
    }
}
