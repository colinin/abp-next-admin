using LINGYUN.Abp.OpenObserve;
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
/// 审计日志 OpenObserve 写入器
/// </summary>
/// <remarks>
/// 审计日志头部写入 <see cref="AbpAuditLoggingOpenObserveOptions.AuditLogStream"/>;
/// 实体变更与操作分别展平写入
/// <see cref="AbpAuditLoggingOpenObserveOptions.GetEntityChangeStream"/> 与
/// <see cref="AbpAuditLoggingOpenObserveOptions.GetActionStream"/>(均由 AuditLogStream 计算得出)
/// </remarks>
[Dependency(ReplaceServices = true)]
public class OpenObserveAuditLogWriter : IAuditLogWriter, ITransientDependency
{
    protected OpenObserveClient Client { get; }
    protected IClock Clock { get; }
    protected IAuditLogInfoToAuditLogConverter AuditLogConverter { get; }
    protected OpenObserveEntityChangeConverter EntityChangeConverter { get; }
    protected IOpenObserveStreamNameNormalizer StreamNameNormalizer { get; }
    protected AbpAuditLoggingOpenObserveOptions Options { get; }

    public ILogger<OpenObserveAuditLogWriter> Logger { protected get; set; }

    public OpenObserveAuditLogWriter(
        OpenObserveClient client,
        IClock clock,
        IAuditLogInfoToAuditLogConverter auditLogConverter,
        OpenObserveEntityChangeConverter entityChangeConverter,
        IOpenObserveStreamNameNormalizer streamNameNormalizer,
        IOptions<AbpAuditLoggingOpenObserveOptions> options)
    {
        Client = client;
        Clock = clock;
        AuditLogConverter = auditLogConverter;
        EntityChangeConverter = entityChangeConverter;
        StreamNameNormalizer = streamNameNormalizer;
        Options = options.Value;

        Logger = NullLogger<OpenObserveAuditLogWriter>.Instance;
    }

    public async virtual Task<string> WriteAsync(AuditLogInfo auditLogInfo, CancellationToken cancellationToken = default)
    {
        var auditLog = await AuditLogConverter.ConvertAsync(auditLogInfo);

        await Client.JsonRecordAsync(
            Options.Organization,
            GetAuditLogStream(auditLog),
            CreateDocument(auditLog),
            cancellationToken);

        await WriteDetailsAsync([auditLog], cancellationToken);

        return auditLog.Id.ToString();
    }

    public async virtual Task BulkWriteAsync(IEnumerable<AuditLogInfo> auditLogInfos, CancellationToken cancellationToken = default)
    {
        var auditLogs = new List<AuditLog>();
        foreach (var auditLogInfo in auditLogInfos)
        {
            auditLogs.Add(await AuditLogConverter.ConvertAsync(auditLogInfo));
        }

        if (auditLogs.Count == 0)
        {
            return;
        }

        // 多租户: 按租户分组写入各自的数据流
        foreach (var group in auditLogs.GroupBy(auditLog => auditLog.TenantId))
        {
            await Client.JsonBulkAsync(
                Options.Organization,
                GetAuditLogStream(group.Key),
                group.Select(CreateDocument),
                cancellationToken: cancellationToken);
        }

        await WriteDetailsAsync(auditLogs, cancellationToken);
    }

    /// <summary>
    /// 获取审计日志数据流名称(按日志所属租户)
    /// </summary>
    protected virtual string GetAuditLogStream(AuditLog auditLog)
    {
        return StreamNameNormalizer.NormalizeStream(Options.AuditLogStream, auditLog.TenantId);
    }

    /// <summary>
    /// 获取审计日志数据流名称(按租户)
    /// </summary>
    protected virtual string GetAuditLogStream(Guid? tenantId)
    {
        return StreamNameNormalizer.NormalizeStream(Options.AuditLogStream, tenantId);
    }

    /// <summary>
    /// 获取实体变更数据流名称(按租户)
    /// </summary>
    protected virtual string GetEntityChangeStream(Guid? tenantId)
    {
        return StreamNameNormalizer.NormalizeStream(Options.GetEntityChangeStream(), tenantId);
    }

    /// <summary>
    /// 获取操作数据流名称(按租户)
    /// </summary>
    protected virtual string GetActionStream(Guid? tenantId)
    {
        return StreamNameNormalizer.NormalizeStream(Options.GetActionStream(), tenantId);
    }

    /// <summary>
    /// 创建审计日志文档(按 ExecutionTime 覆盖 _timestamp)
    /// </summary>
    protected virtual OpenObserveAuditLog CreateDocument(AuditLog auditLog)
    {
        var document = OpenObserveAuditLog.From(auditLog);

        if (Options.UseLogTimeAsTimestamp)
        {
            document.Timestamp = OpenObserveTimestampHelper.TryGetTimestamp(
                auditLog.ExecutionTime,
                Clock.Now,
                Options.TimestampAllowedUptoHours);
        }

        return document;
    }

    /// <summary>
    /// 展平写入实体变更与操作
    /// </summary>
    protected async virtual Task WriteDetailsAsync(
        IReadOnlyList<AuditLog> auditLogs,
        CancellationToken cancellationToken = default)
    {
        if (Options.WriteEntityChanges)
        {
            var entityChanges = auditLogs
                .SelectMany(EntityChangeConverter.ToDocuments)
                .ToList();

            if (Options.UseLogTimeAsTimestamp)
            {
                foreach (var entityChange in entityChanges)
                {
                    entityChange.Timestamp = OpenObserveTimestampHelper.TryGetTimestamp(
                        entityChange.ChangeTime,
                        Clock.Now,
                        Options.TimestampAllowedUptoHours);
                }
            }

            if (entityChanges.Count > 0)
            {
                foreach (var group in entityChanges.GroupBy(entityChange => entityChange.TenantId))
                {
                    await Client.JsonBulkAsync(
                        Options.Organization,
                        GetEntityChangeStream(group.Key),
                        group,
                        cancellationToken: cancellationToken);
                }
            }
        }

        if (Options.WriteActions)
        {
            var actions = auditLogs
                .SelectMany(OpenObserveAuditLogAction.CreateMany)
                .ToList();

            if (Options.UseLogTimeAsTimestamp)
            {
                foreach (var action in actions)
                {
                    action.Timestamp = OpenObserveTimestampHelper.TryGetTimestamp(
                        action.ExecutionTime,
                        Clock.Now,
                        Options.TimestampAllowedUptoHours);
                }
            }

            if (actions.Count > 0)
            {
                foreach (var group in actions.GroupBy(action => action.TenantId))
                {
                    await Client.JsonBulkAsync(
                        Options.Organization,
                        GetActionStream(group.Key),
                        group,
                        cancellationToken: cancellationToken);
                }
            }
        }
    }
}
