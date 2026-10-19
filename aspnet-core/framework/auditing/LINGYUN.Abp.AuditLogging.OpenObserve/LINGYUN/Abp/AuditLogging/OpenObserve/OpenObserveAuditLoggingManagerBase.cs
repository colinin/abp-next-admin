using LINGYUN.Abp.OpenObserve;
using LINGYUN.Abp.OpenObserve.Models;
using LINGYUN.Abp.OpenObserve.Models.Search;
using LINGYUN.Abp.OpenObserve.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.AuditLogging.OpenObserve;

/// <summary>
/// 审计日志 OpenObserve 管理器基类
/// </summary>
/// <remarks>
/// OpenObserve 的几个约束在此统一处理:
/// * 查询必须指定时间范围, 且范围作用于写入时间(_timestamp), 因此统一使用宽窗口,
///   业务时间过滤由 SQL 条件完成(列名小写, 时间为 ISO 8601 字符串);
/// * 不支持按记录删除, 删除接口直接抛出 <see cref="NotSupportedException"/>;
/// * 列名一律小写(OpenObserve 会小写化字段名)。
/// </remarks>
public abstract class OpenObserveAuditLoggingManagerBase
{
    /// <summary>
    /// 单次查询最多返回的记录数
    /// </summary>
    public const int MaxSearchResultCount = 5000;

    protected IClock Clock { get; }
    protected OpenObserveClient Client { get; }
    protected AbpAuditLoggingOpenObserveOptions Options { get; }
    protected OpenObserveSqlBuilder SqlBuilder { get; }
    protected IOpenObserveStreamNameNormalizer StreamNameNormalizer { get; }
    protected ILogger Logger { get; }

    protected OpenObserveAuditLoggingManagerBase(
        IClock clock,
        OpenObserveClient client,
        AbpAuditLoggingOpenObserveOptions options,
        OpenObserveSqlBuilder sqlBuilder,
        IOpenObserveStreamNameNormalizer streamNameNormalizer,
        ILogger logger)
    {
        Clock = clock;
        Client = client;
        Options = options;
        SqlBuilder = sqlBuilder;
        StreamNameNormalizer = streamNameNormalizer;
        Logger = logger;
    }

    /// <summary>
    /// 获取审计日志数据流名称(按租户)
    /// </summary>
    protected virtual string GetAuditLogStream(Guid? tenantId = null)
    {
        return StreamNameNormalizer.NormalizeStream(Options.AuditLogStream, tenantId);
    }

    /// <summary>
    /// 获取安全日志数据流名称(按租户)
    /// </summary>
    protected virtual string GetSecurityLogStream(Guid? tenantId = null)
    {
        return StreamNameNormalizer.NormalizeStream(Options.SecurityLogStream, tenantId);
    }

    /// <summary>
    /// 获取实体变更数据流名称(按租户)
    /// </summary>
    protected virtual string GetEntityChangeStream(Guid? tenantId = null)
    {
        return StreamNameNormalizer.NormalizeStream(Options.GetEntityChangeStream(), tenantId);
    }

    /// <summary>
    /// 获取操作数据流名称(按租户)
    /// </summary>
    protected virtual string GetActionStream(Guid? tenantId = null)
    {
        return StreamNameNormalizer.NormalizeStream(Options.GetActionStream(), tenantId);
    }

    /// <summary>
    /// 查询窗口开始时间
    /// </summary>
    /// <remarks>
    /// 作用域是记录的写入时间(_timestamp), 业务时间请使用 SQL 条件过滤
    /// </remarks>
    protected virtual DateTime GetQueryStartTime()
    {
        return Clock.Now.AddDays(-Math.Abs(Options.DefaultQueryDays));
    }

    /// <summary>
    /// 查询窗口结束时间
    /// </summary>
    protected virtual DateTime GetQueryEndTime()
    {
        return Clock.Now.AddDays(1);
    }

    /// <summary>
    /// 执行 SQL 查询
    /// </summary>
    /// <remarks>
    /// OpenObserve 在数据流不存在时返回错误(尚未写入过数据), 此处按空结果处理;
    /// 其它错误(凭据、语法、服务异常)继续向上抛出
    /// </remarks>
    protected async virtual Task<SearchResponse<THit>> SearchAsync<THit>(
        string sql,
        int skipCount = 0,
        int maxResultCount = 50,
        CancellationToken cancellationToken = default)
    {
        var query = new SearchQuery(
            sql,
            GetQueryStartTime(),
            GetQueryEndTime(),
            skipCount,
            maxResultCount);

        try
        {
            return await Client.SearchAsync<THit>(
                Options.Organization,
                query,
                cancellationToken: cancellationToken);
        }
        catch (OpenObserveException exception) when (IsStreamNotFound(exception))
        {
            Logger.LogWarning(
                "The OpenObserve stream does not exist yet (no data has been ingested), an empty result is returned. {Message}",
                exception.Message);

            return new SearchResponse<THit>();
        }
    }

    /// <summary>
    /// 是否为"数据流不存在"错误
    /// </summary>
    /// <remarks>
    /// OpenObserve 未写入过数据时数据流尚未创建, 返回 code 20002 与
    /// <c>Search stream not found: {stream}</c> 消息
    /// </remarks>
    protected virtual bool IsStreamNotFound(OpenObserveException exception)
    {
        return exception.Message.Contains("stream not found", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 执行 SQL 查询并返回全部命中
    /// </summary>
    protected async virtual Task<List<THit>> SearchAllAsync<THit>(
        string sql,
        int maxResultCount = MaxSearchResultCount,
        CancellationToken cancellationToken = default)
    {
        var response = await SearchAsync<THit>(sql, 0, maxResultCount, cancellationToken);

        return response.Hits.ToList();
    }

    /// <summary>
    /// 统计符合条件的记录数
    /// </summary>
    protected async virtual Task<long> GetTotalAsync(
        string stream,
        string condition,
        CancellationToken cancellationToken = default)
    {
        var sql = $"select count(*) as total from {stream} where {condition}";

        var response = await SearchAsync<OpenObserveCount>(sql, 0, 1, cancellationToken);

        return response.Hits.Length == 0 ? 0 : response.Hits[0].Total;
    }

    /// <summary>
    /// 构建条件
    /// </summary>
    protected virtual string BuildCondition(IEnumerable<Condition> conditions)
    {
        return SqlBuilder.Build(conditions);
    }

    /// <summary>
    /// 构建时间范围条件
    /// </summary>
    /// <remarks>
    /// 时间列以 ISO 8601 字符串存储, 按相同格式比较
    /// </remarks>
    protected virtual string BuildTimeCondition(string field, DateTime? startTime, DateTime? endTime)
    {
        var parts = new List<string>();

        if (startTime.HasValue)
        {
            parts.Add($"{field} >= {SqlBuilder.FormatDateTime(startTime.Value)}");
        }
        if (endTime.HasValue)
        {
            parts.Add($"{field} <= {SqlBuilder.FormatDateTime(endTime.Value)}");
        }

        return string.Join(" AND ", parts);
    }

    /// <summary>
    /// 组合条件
    /// </summary>
    protected virtual string Combine(params string[] conditions)
    {
        var parts = conditions
            .Where(condition => !condition.IsNullOrWhiteSpace())
            .ToList();

        return parts.Count == 0 ? "1 = 1" : string.Join(" AND ", parts);
    }

    /// <summary>
    /// 归一化排序字段(表达式成员名 -> 数据流列名)
    /// </summary>
    protected virtual string NormalizeSorting(string? sorting, string defaultSorting)
    {
        var value = sorting.IsNullOrWhiteSpace() ? defaultSorting : sorting!;

        var parts = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length > 0)
        {
            parts[0] = SqlBuilder.GetMappedField(parts[0]);
        }

        return string.Join(' ', parts);
    }

    /// <summary>
    /// 转义字符串
    /// </summary>
    protected virtual string Escape(string? value)
    {
        return SqlBuilder.Escape(value ?? string.Empty);
    }
}
