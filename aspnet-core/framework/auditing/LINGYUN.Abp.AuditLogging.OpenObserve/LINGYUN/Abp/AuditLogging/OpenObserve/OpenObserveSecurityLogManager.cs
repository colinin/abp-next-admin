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
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.AuditLogging.OpenObserve;

/// <summary>
/// 安全日志 OpenObserve 管理器
/// </summary>
/// <remarks>
/// OpenObserve 的限制与处理方式参见 <see cref="OpenObserveAuditLoggingManagerBase"/>
/// </remarks>
[Dependency(ReplaceServices = true)]
public class OpenObserveSecurityLogManager : OpenObserveAuditLoggingManagerBase, ISecurityLogManager, ITransientDependency
{
    /// <summary>
    /// 表达式成员名 -> 数据流列名(数据流列名一律小写)
    /// </summary>
    protected readonly static Dictionary<string, string> MapedFields = new(StringComparer.OrdinalIgnoreCase)
    {
        { nameof(SecurityLog.Id), "id" },
        { nameof(SecurityLog.TenantId), "tenantid" },
        { nameof(SecurityLog.TenantName), "tenantname" },
        { nameof(SecurityLog.ApplicationName), "applicationname" },
        { nameof(SecurityLog.Identity), "identity" },
        { nameof(SecurityLog.Action), "action" },
        { nameof(SecurityLog.UserId), "userid" },
        { nameof(SecurityLog.UserName), "username" },
        { nameof(SecurityLog.ClientId), "clientid" },
        { nameof(SecurityLog.CorrelationId), "correlationid" },
        { nameof(SecurityLog.ClientIpAddress), "clientipaddress" },
        { nameof(SecurityLog.BrowserInfo), "browserinfo" },
        { nameof(SecurityLog.CreationTime), "creationtime" },
    };

    public OpenObserveSecurityLogManager(
        IClock clock,
        OpenObserveClient client,
        IOpenObserveStreamNameNormalizer streamNameNormalizer,
        IOptions<AbpAuditLoggingOpenObserveOptions> options,
        ILogger<OpenObserveSecurityLogManager> logger)
        : base(clock, client, options.Value, CreateSqlBuilder(), streamNameNormalizer, logger)
    {
    }

    public async virtual Task<SecurityLog?> GetAsync(
        Guid id,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var condition = $"id = '{id:D}'";

        var sql = $"select * from {GetSecurityLogStream()} where {condition}";

        var hits = await SearchAllAsync<OpenObserveSecurityLog>(sql, 1, cancellationToken);

        return hits.FirstOrDefault();
    }

    public virtual Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw CreateDeleteNotSupportedException();
    }

    public virtual Task DeleteManyAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    {
        throw CreateDeleteNotSupportedException();
    }

    /// <summary>
    /// OpenObserve 不支持删除记录
    /// </summary>
    protected virtual BusinessException CreateDeleteNotSupportedException()
    {
        return new BusinessException(AuditLoggingOpenObserveErrorCodes.DeleteNotSupported);
    }

    public async virtual Task<List<SecurityLog>> GetListAsync(
        string? sorting = null,
        int maxResultCount = 50,
        int skipCount = 0,
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? applicationName = null,
        string? identity = null,
        string? action = null,
        Guid? userId = null,
        string? userName = null,
        string? clientId = null,
        string? clientIpAddress = null,
        string? correlationId = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var conditions = BuildConditions(
            applicationName,
            identity,
            action,
            userId,
            userName,
            clientId,
            clientIpAddress,
            correlationId);

        var condition = Combine(
            BuildCondition(conditions),
            BuildTimeCondition("creationtime", startTime, endTime));

        var sql = $"select * from {GetSecurityLogStream()} where {condition} " +
                  $"order by {NormalizeSorting(sorting, $"{nameof(SecurityLog.CreationTime)} DESC")}";

        var hits = await SearchAllAsync<OpenObserveSecurityLog>(sql, maxResultCount, cancellationToken);

        return hits
            .Cast<SecurityLog>()
            .ToList();
    }

    public async virtual Task<long> GetCountAsync(
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? applicationName = null,
        string? identity = null,
        string? action = null,
        Guid? userId = null,
        string? userName = null,
        string? clientId = null,
        string? clientIpAddress = null,
        string? correlationId = null,
        CancellationToken cancellationToken = default)
    {
        var conditions = BuildConditions(
            applicationName,
            identity,
            action,
            userId,
            userName,
            clientId,
            clientIpAddress,
            correlationId);

        var condition = Combine(
            BuildCondition(conditions),
            BuildTimeCondition("creationtime", startTime, endTime));

        return await GetTotalAsync(GetSecurityLogStream(), condition, cancellationToken);
    }

    /// <summary>
    /// 由查询参数构建条件
    /// </summary>
    protected virtual List<Condition> BuildConditions(
        string? applicationName = null,
        string? identity = null,
        string? action = null,
        Guid? userId = null,
        string? userName = null,
        string? clientId = null,
        string? clientIpAddress = null,
        string? correlationId = null)
    {
        var conditions = new List<Condition>();

        if (!applicationName.IsNullOrWhiteSpace())
        {
            conditions.Add(new Condition(nameof(SecurityLog.ApplicationName), ComparisonOp.Contains, applicationName));
        }
        if (!identity.IsNullOrWhiteSpace())
        {
            conditions.Add(new Condition(nameof(SecurityLog.Identity), ComparisonOp.Contains, identity));
        }
        if (!action.IsNullOrWhiteSpace())
        {
            conditions.Add(new Condition(nameof(SecurityLog.Action), ComparisonOp.Equal, action));
        }
        if (userId.HasValue)
        {
            conditions.Add(new Condition(nameof(SecurityLog.UserId), ComparisonOp.Equal, userId.Value));
        }
        if (!userName.IsNullOrWhiteSpace())
        {
            conditions.Add(new Condition(nameof(SecurityLog.UserName), ComparisonOp.Contains, userName));
        }
        if (!clientId.IsNullOrWhiteSpace())
        {
            conditions.Add(new Condition(nameof(SecurityLog.ClientId), ComparisonOp.Equal, clientId));
        }
        if (!clientIpAddress.IsNullOrWhiteSpace())
        {
            conditions.Add(new Condition(nameof(SecurityLog.ClientIpAddress), ComparisonOp.Equal, clientIpAddress));
        }
        if (!correlationId.IsNullOrWhiteSpace())
        {
            conditions.Add(new Condition(nameof(SecurityLog.CorrelationId), ComparisonOp.Equal, correlationId));
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
