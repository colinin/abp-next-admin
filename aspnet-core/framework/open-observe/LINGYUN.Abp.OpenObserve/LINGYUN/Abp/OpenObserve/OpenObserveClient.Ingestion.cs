using LINGYUN.Abp.OpenObserve.Models;
using LINGYUN.Abp.OpenObserve.Models.Ingestion;
using System.Net.Http.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OpenObserve;

public partial class OpenObserveClient
{
    /// <summary>
    /// 日志摄取 - JSON
    /// </summary>
    /// <remarks>
    /// POST /api/{organization}/{stream}/_json
    /// 请求体为 JSON 数组: [ {..}, {..} ]
    /// 响应: {"code":200,"status":[{"name":"stream1","successful":2,"failed":0}]}
    /// 参考: https://openobserve.ai/docs/reference/api/ingestion/logs/json/
    /// </remarks>
    public async virtual Task<JsonResponse> JsonRequestAsync<TData>(
        string? organization,
        string stream,
        IEnumerable<TData> datas,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/{ResolveOrganization(organization)}/{Uri.EscapeDataString(stream)}/_json";

        var httpResponse = await HttpClient.PostAsync(requestUri, CreateJsonContent(datas), cancellationToken);

        await EnsureSuccessStatusCode(httpResponse, ThrowIfIngestionException(stream), cancellationToken);

        var responseContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

        return DeserializeOrDefault<JsonResponse>(responseContent)!;
    }

    /// <summary>
    /// 日志摄取 - JSON(单条记录)
    /// </summary>
    /// <remarks>
    /// 单条记录使用独立方法名, 避免与 <see cref="JsonRequestAsync{TData}(string?, string, IEnumerable{TData}, CancellationToken)"/> 产生重载歧义
    /// (传入数组时会优先命中泛型单条重载, 导致请求体被多包一层数组)
    /// </remarks>
    public async virtual Task<JsonResponse> JsonRecordAsync<TData>(
        string? organization,
        string stream,
        TData data,
        CancellationToken cancellationToken = default)
    {
        return await JsonRequestAsync(organization, stream, [data], cancellationToken);
    }

    /// <summary>
    /// 日志摄取 - Bulk
    /// </summary>
    /// <remarks>
    /// POST /api/{organization}/_bulk
    /// 请求体为 NDJSON, 每两条为一组: 第一行为动作行 {"index":{"_index":"stream1"}}, 第二行为记录数据
    /// 响应为 Elasticsearch _bulk 兼容结构: {"took":0,"errors":false,"items":[{"index":{...}}]}
    /// 参考: https://openobserve.ai/docs/reference/api/ingestion/logs/bulk/
    /// </remarks>
    /// <param name="action">动作: index / create / update, 默认 index</param>
    public async virtual Task<JsonBulkResponse> JsonBulkAsync<TData>(
        string? organization,
        string stream,
        IEnumerable<TData> datas,
        string action = OpenObserveConstants.BulkActions.Index,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/{ResolveOrganization(organization)}/_bulk";

        var builder = new StringBuilder();
        foreach (var data in datas)
        {
            // 动作行: {"index":{"_index":"stream1"}}
            builder.AppendLine(Serializer.Serialize(
                new Dictionary<string, JsonBulkRequest>
                {
                    [action] = new JsonBulkRequest { Index = stream },
                }));
            // 记录数据行
            builder.AppendLine(Serializer.Serialize(data));
        }

        using var content = new StringContent(builder.ToString(), Encoding.UTF8, OpenObserveConstants.NdJsonContentType);

        var httpResponse = await HttpClient.PostAsync(requestUri, content, cancellationToken);

        await EnsureSuccessStatusCode(httpResponse, ThrowIfIngestionException(stream), cancellationToken);

        var responseContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

        return DeserializeOrDefault<JsonBulkResponse>(responseContent)!;
    }

    /// <summary>
    /// 日志摄取 - Multi
    /// </summary>
    /// <remarks>
    /// POST /api/{organization}/{stream}/_multi
    /// 请求体为 NDJSON, 每行一个记录对象(没有动作行)
    /// 响应与 _json 相同: {"code":200,"status":[{"name":"stream1","successful":2,"failed":0}]}
    /// 参考: https://openobserve.ai/docs/reference/api/ingestion/logs/multi/
    /// </remarks>
    public async virtual Task<JsonResponse> JsonMultiAsync<TData>(
        string? organization,
        string stream,
        IEnumerable<TData> datas,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/{ResolveOrganization(organization)}/{Uri.EscapeDataString(stream)}/_multi";

        var builder = new StringBuilder();
        foreach (var data in datas)
        {
            builder.AppendLine(Serializer.Serialize(data));
        }

        using var content = new StringContent(builder.ToString(), Encoding.UTF8, OpenObserveConstants.NdJsonContentType);

        var httpResponse = await HttpClient.PostAsync(requestUri, content, cancellationToken);

        await EnsureSuccessStatusCode(httpResponse, ThrowIfIngestionException(stream), cancellationToken);

        var responseContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

        return DeserializeOrDefault<JsonResponse>(responseContent)!;
    }

    /// <summary>
    /// 摄取失败时创建异常
    /// </summary>
    protected virtual Func<OpenObserveErrorResponse, HttpResponseMessage, Exception> ThrowIfIngestionException(string stream)
    {
        return (errorResponse, httpResponse) => new OpenObserveIngestionException(
            stream,
            errorResponse,
            (int)httpResponse.StatusCode);
    }
}
