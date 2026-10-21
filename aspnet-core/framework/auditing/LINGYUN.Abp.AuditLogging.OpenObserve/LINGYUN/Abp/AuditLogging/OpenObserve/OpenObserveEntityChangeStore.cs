using LINGYUN.Abp.OpenObserve;
using LINGYUN.Abp.OpenObserve.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.AuditLogging.OpenObserve;

/// <summary>
/// 实体变更 OpenObserve 存储
/// </summary>
/// <remarks>
/// 实体变更被展平写入 <see cref="AbpAuditLoggingOpenObserveOptions.GetEntityChangeStream"/>,
/// 因此可直接按实体字段检索
/// </remarks>
[Dependency(ReplaceServices = true)]
public class OpenObserveEntityChangeStore : OpenObserveAuditLoggingManagerBase, IEntityChangeStore, ITransientDependency
{
    /// <summary>
    /// 表达式成员名 -> 数据流列名(数据流列名一律小写)
    /// </summary>
    protected readonly static Dictionary<string, string> MapedFields = new(StringComparer.OrdinalIgnoreCase)
    {
        { nameof(EntityChange.Id), "id" },
        { nameof(EntityChange.AuditLogId), "auditlogid" },
        { nameof(EntityChange.TenantId), "tenantid" },
        { nameof(EntityChange.EntityTenantId), "entitytenantid" },
        { nameof(EntityChange.ChangeTime), "changetime" },
        { nameof(EntityChange.ChangeType), "changetype" },
        { nameof(EntityChange.EntityId), "entityid" },
        { nameof(EntityChange.EntityTypeFullName), "entitytypefullname" },
        { nameof(EntityChangeWithUsername.UserName), "username" },
    };

    protected OpenObserveEntityChangeConverter EntityChangeConverter { get; }

    public OpenObserveEntityChangeStore(
        IClock clock,
        OpenObserveClient client,
        OpenObserveEntityChangeConverter entityChangeConverter,
        IOpenObserveStreamNameNormalizer streamNameNormalizer,
        IOptions<AbpAuditLoggingOpenObserveOptions> options,
        ILogger<OpenObserveEntityChangeStore> logger)
        : base(clock, client, options.Value, CreateSqlBuilder(), streamNameNormalizer, logger)
    {
        EntityChangeConverter = entityChangeConverter;
    }

    public async virtual Task<EntityChange?> GetAsync(
        Guid entityChangeId,
        CancellationToken cancellationToken = default)
    {
        var change = (await FindAsync(
            $"id = '{entityChangeId:D}'",
            maxResultCount: 1,
            cancellationToken: cancellationToken))
            .FirstOrDefault();

        if (change == null)
        {
            return null;
        }

        return EntityChangeConverter.ToEntityChange(change);
    }

    public async virtual Task<long> GetCountAsync(
        Guid? auditLogId = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        EntityChangeType? changeType = null,
        string? entityId = null,
        string? entityTypeFullName = null,
        CancellationToken cancellationToken = default)
    {
        var conditions = BuildConditions(auditLogId, changeType, entityId, entityTypeFullName);

        var condition = Combine(
            BuildCondition(conditions),
            BuildTimeCondition("changetime", startTime, endTime));

        return await GetTotalAsync(GetEntityChangeStream(), condition, cancellationToken);
    }

    public async virtual Task<List<EntityChange>> GetListAsync(
        string? sorting = null,
        int maxResultCount = 50,
        int skipCount = 0,
        Guid? auditLogId = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        EntityChangeType? changeType = null,
        string? entityId = null,
        string? entityTypeFullName = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var conditions = BuildConditions(auditLogId, changeType, entityId, entityTypeFullName);

        var condition = Combine(
            BuildCondition(conditions),
            BuildTimeCondition("changetime", startTime, endTime));

        var changes = await FindAsync(condition, sorting, skipCount, maxResultCount, cancellationToken);

        return changes
            .Select(EntityChangeConverter.ToEntityChange)
            .ToList();
    }

    public async virtual Task<EntityChangeWithUsername?> GetWithUsernameAsync(
        Guid entityChangeId,
        CancellationToken cancellationToken = default)
    {
        var change = (await FindAsync(
            $"id = '{entityChangeId:D}'",
            maxResultCount: 1,
            cancellationToken: cancellationToken))
            .FirstOrDefault();

        if (change == null)
        {
            return null;
        }

        return EntityChangeConverter.ToEntityChangeWithUsername(change);
    }

    public async virtual Task<List<EntityChangeWithUsername>> GetWithUsernameAsync(
        string entityId,
        string entityTypeFullName,
        CancellationToken cancellationToken = default)
    {
        var conditions = new List<Condition>
        {
            new(nameof(EntityChange.EntityId), ComparisonOp.Equal, entityId),
            new(nameof(EntityChange.EntityTypeFullName), ComparisonOp.Equal, entityTypeFullName),
        };

        var condition = BuildCondition(conditions);

        var changes = await FindAsync(condition, maxResultCount: 1000, cancellationToken: cancellationToken);

        return changes
            .Select(EntityChangeConverter.ToEntityChangeWithUsername)
            .ToList();
    }

    /// <summary>
    /// 查询实体变更
    /// </summary>
    protected async virtual Task<List<OpenObserveEntityChange>> FindAsync(
        string condition,
        string? sorting = null,
        int skipCount = 0,
        int maxResultCount = 50,
        CancellationToken cancellationToken = default)
    {
        var sql = $"select * from {GetEntityChangeStream()} where {condition} " +
                  $"order by {NormalizeSorting(sorting, $"{nameof(EntityChange.ChangeTime)} DESC")}";

        var hits = await SearchAsync<OpenObserveEntityChange>(sql, skipCount, maxResultCount, cancellationToken);

        return hits.Hits.ToList();
    }

    /// <summary>
    /// 由查询参数构建条件
    /// </summary>
    protected virtual List<Condition> BuildConditions(
        Guid? auditLogId = null,
        EntityChangeType? changeType = null,
        string? entityId = null,
        string? entityTypeFullName = null)
    {
        var conditions = new List<Condition>();

        if (auditLogId.HasValue)
        {
            conditions.Add(new Condition(nameof(EntityChange.AuditLogId), ComparisonOp.Equal, auditLogId.Value));
        }
        if (changeType.HasValue)
        {
            conditions.Add(new Condition(nameof(EntityChange.ChangeType), ComparisonOp.Equal, changeType.Value.ToString()));
        }
        if (!entityId.IsNullOrWhiteSpace())
        {
            conditions.Add(new Condition(nameof(EntityChange.EntityId), ComparisonOp.Equal, entityId));
        }
        if (!entityTypeFullName.IsNullOrWhiteSpace())
        {
            conditions.Add(new Condition(nameof(EntityChange.EntityTypeFullName), ComparisonOp.Equal, entityTypeFullName));
        }

        return conditions;
    }

    protected static OpenObserveSqlBuilder CreateSqlBuilder()
    {
        var sqlBuilder = new OpenObserveSqlBuilder();

        foreach (var field in MapedFields)
        {
            sqlBuilder.MapField(field.Key, field.Value);
        }

        return sqlBuilder;
    }
}
