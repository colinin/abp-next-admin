using LINGYUN.Abp.OpenObserve.Models;
using LINGYUN.Abp.OpenObserve.Models.Stream;
using LINGYUN.Abp.OpenObserve.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OpenObserve;

public partial class OpenObserveClient
{
    /// <summary>
    /// 列出数据流
    /// </summary>
    /// <remarks>
    /// GET /api/{organization}/streams
    /// 参考: https://openobserve.ai/docs/reference/api/stream/list/
    /// </remarks>
    /// <param name="fetchSchema">是否返回每个数据流的字段定义</param>
    /// <param name="type">数据流类型, 默认 logs</param>
    public async virtual Task<StreamListResponse> GetStreamsAsync(
        string? organization = null,
        bool? fetchSchema = null,
        StreamType? type = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = OpenObserveQueryString.Build(
            ("fetchSchema", fetchSchema),
            ("type", type?.AsString()));

        var requestUri = $"/api/{ResolveOrganization(organization)}/streams{queryString}";

        return (await GetAsync<StreamListResponse>(requestUri, cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 获取数据流字段定义
    /// </summary>
    /// <remarks>
    /// GET /api/{organization}/streams/{stream}/schema
    /// 参考: https://openobserve.ai/docs/reference/api/stream/schema/
    /// </remarks>
    public async virtual Task<StreamInfo> GetStreamSchemaAsync(
        string stream,
        string? organization = null,
        StreamType? type = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = OpenObserveQueryString.Build(("type", type?.AsString()));

        var requestUri = $"/api/{ResolveOrganization(organization)}/streams/{Uri.EscapeDataString(stream)}/schema{queryString}";

        return (await GetAsync<StreamInfo>(requestUri, cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 创建数据流设置
    /// </summary>
    /// <remarks>
    /// POST /api/{org_id}/streams/{stream_name}/settings
    /// 参考: https://openobserve.ai/docs/reference/api/stream/setting/
    /// </remarks>
    /// <param name="partitionKeys">分区字段</param>
    /// <param name="indexFields">二级索引字段</param>
    /// <param name="fullTextSearchKeys">全文检索字段</param>
    /// <param name="bloomFilterFields">布隆过滤器字段</param>
    /// <param name="dataRetention">数据保留天数, 最小 3 天</param>
    /// <param name="flattenLevel">扁平化层级</param>
    /// <param name="definedSchemaFields">保留的自定义 schema 字段</param>
    /// <param name="maxQueryRange">单次查询最大时间范围(小时)</param>
    /// <param name="storeOriginalData">是否存储原始数据</param>
    /// <param name="approxPartition">是否使用均分时间范围分区</param>
    /// <param name="extendedRetentionDays">延长保留期的时间范围</param>
    /// <param name="indexOriginalData">是否对原始日志体建立全文索引</param>
    /// <param name="indexAllValues">是否为所有字段建立精确匹配索引</param>
    /// <param name="storageType">存储类型: normal(默认) / compliance</param>
    public async virtual Task<OpenObserveCodeMessageResponse> CreateStreamSettingsAsync(
        string stream,
        string[]? partitionKeys = null,
        string[]? indexFields = null,
        string[]? fullTextSearchKeys = null,
        string[]? bloomFilterFields = null,
        int? dataRetention = null,
        int? flattenLevel = null,
        string[]? definedSchemaFields = null,
        int? maxQueryRange = null,
        bool? storeOriginalData = null,
        bool? approxPartition = null,
        object[]? extendedRetentionDays = null,
        bool? indexOriginalData = null,
        bool? indexAllValues = null,
        string? storageType = null,
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/{ResolveOrganization(organization)}/streams/{Uri.EscapeDataString(stream)}/settings";

        var request = new StreamSettingRequest
        {
            PartitionKeys = partitionKeys,
            IndexFields = indexFields,
            FullTextSearchKeys = fullTextSearchKeys,
            BloomFilterFields = bloomFilterFields,
            DataRetention = dataRetention,
            FlattenLevel = flattenLevel,
            DefinedSchemaFields = definedSchemaFields,
            MaxQueryRange = maxQueryRange,
            StoreOriginalData = storeOriginalData,
            ApproxPartition = approxPartition,
            ExtendedRetentionDays = extendedRetentionDays,
            IndexOriginalData = indexOriginalData,
            IndexAllValues = indexAllValues,
            StorageType = storageType,
        };

        return (await PostJsonAsync<StreamSettingRequest, OpenObserveCodeMessageResponse>(
            requestUri,
            request,
            cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 更新数据流设置
    /// </summary>
    /// <remarks>
    /// PUT /api/{org_id}/streams/{stream_name}/settings
    /// 支持 add/set/remove 的字段使用 <see cref="UpdateStreamSettingValue{T}"/> 传递
    /// 参考: https://openobserve.ai/docs/reference/api/stream/setting/
    /// </remarks>
    public async virtual Task<OpenObserveCodeMessageResponse> UpdateStreamSettingsAsync(
        string stream,
        UpdateStreamSettingValue<string>? partitionKeys = null,
        UpdateStreamSettingValue<string>? indexFields = null,
        UpdateStreamSettingValue<string>? fullTextSearchKeys = null,
        UpdateStreamSettingValue<string>? bloomFilterFields = null,
        UpdateStreamSettingValue<string>? definedSchemaFields = null,
        UpdateStreamSettingValue<object>? extendedRetentionDays = null,
        int? dataRetention = null,
        int? flattenLevel = null,
        int? maxQueryRange = null,
        bool? storeOriginalData = null,
        bool? approxPartition = null,
        bool? indexOriginalData = null,
        bool? indexAllValues = null,
        string? storageType = null,
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/{ResolveOrganization(organization)}/streams/{Uri.EscapeDataString(stream)}/settings";

        var request = new UpdateStreamSettingRequest
        {
            PartitionKeys = partitionKeys,
            IndexFields = indexFields,
            FullTextSearchKeys = fullTextSearchKeys,
            BloomFilterFields = bloomFilterFields,
            DefinedSchemaFields = definedSchemaFields,
            ExtendedRetentionDays = extendedRetentionDays,
            DataRetention = dataRetention,
            FlattenLevel = flattenLevel,
            MaxQueryRange = maxQueryRange,
            StoreOriginalData = storeOriginalData,
            ApproxPartition = approxPartition,
            IndexOriginalData = indexOriginalData,
            IndexAllValues = indexAllValues,
            StorageType = storageType,
        };

        return (await PutJsonAsync<UpdateStreamSettingRequest, OpenObserveCodeMessageResponse>(
            requestUri,
            request,
            cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 删除数据流
    /// </summary>
    /// <remarks>
    /// DELETE /api/{org_id}/streams/{stream_name}?type=logs&amp;delete_all=true
    /// 删除是异步的(Compactor 每 10 分钟轮询), 且不可恢复
    /// 参考: https://openobserve.ai/docs/reference/api/stream/delete/
    /// </remarks>
    /// <param name="deleteAll">是否同时删除关联的告警与仪表盘</param>
    public async virtual Task<OpenObserveCodeMessageResponse> DeleteStreamAsync(
        string stream,
        StreamType type = StreamType.Logs,
        bool deleteAll = false,
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = OpenObserveQueryString.Build(
            ("type", type.AsString()),
            ("delete_all", deleteAll));

        var requestUri = $"/api/{ResolveOrganization(organization)}/streams/{Uri.EscapeDataString(stream)}{queryString}";

        return (await DeleteAsync<OpenObserveCodeMessageResponse>(requestUri, cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 按时间范围删除数据
    /// </summary>
    /// <remarks>
    /// DELETE /api/{org_id}/streams/{stream_name}/data_by_time_range?start=..&amp;end=..
    /// 日志按小时删除, 链路按天删除
    /// </remarks>
    /// <param name="startTime">开始时间(含)</param>
    /// <param name="endTime">结束时间(含)</param>
    public async virtual Task<StreamDeleteJobResponse> DeleteStreamDataByTimeRangeAsync(
        string stream,
        DateTime startTime,
        DateTime endTime,
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = OpenObserveQueryString.Build(
            ("start", startTime.ToMicroseconds()),
            ("end", endTime.ToMicroseconds()));

        var requestUri = $"/api/{ResolveOrganization(organization)}/streams/{Uri.EscapeDataString(stream)}/data_by_time_range{queryString}";

        return (await DeleteAsync<StreamDeleteJobResponse>(requestUri, cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 查询按时间范围删除数据的任务状态
    /// </summary>
    /// <remarks>
    /// GET /api/{org_id}/streams/{stream_name}/data_by_time_range/status/{id}
    /// </remarks>
    public async virtual Task<StreamDeleteJobStatusResponse> GetStreamDataByTimeRangeStatusAsync(
        string stream,
        string id,
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/{ResolveOrganization(organization)}/streams/{Uri.EscapeDataString(stream)}/data_by_time_range/status/{Uri.EscapeDataString(id)}";

        return (await GetAsync<StreamDeleteJobStatusResponse>(requestUri, cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 删除数据流结果缓存
    /// </summary>
    /// <remarks>
    /// DELETE /api/{org_id}/streams/{stream_name}/cache/results?type=..&amp;ts=..
    /// 删除 ts 之前的缓存, ts 之后(含)的缓存保留
    /// </remarks>
    /// <param name="stream">数据流名称, 传入 _all 时删除所有数据流的缓存</param>
    /// <param name="timestamp">时间阈值</param>
    public async virtual Task<OpenObserveCodeMessageResponse> DeleteStreamCacheAsync(
        string stream,
        StreamType type,
        DateTime timestamp,
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = OpenObserveQueryString.Build(
            ("type", type.AsString()),
            ("ts", timestamp.ToMicroseconds()));

        var requestUri = $"/api/{ResolveOrganization(organization)}/streams/{Uri.EscapeDataString(stream)}/cache/results{queryString}";

        return (await DeleteAsync<OpenObserveCodeMessageResponse>(requestUri, cancellationToken: cancellationToken))!;
    }
}
