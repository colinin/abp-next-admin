using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Volo.Abp.Data;

namespace LINGYUN.Abp.AuditLogging.OpenObserve;

/// <summary>
/// 展平的操作记录(写入 OpenObserve 操作数据流)
/// </summary>
/// <remarks>
/// <see cref="AuditLogAction.Parameters"/> 在大数据量操作下可能很大, 因此操作被展平写入
/// <c>{AuditLogStream}_actions</c> 独立数据流; 查询时先在该数据流匹配 <c>auditlogid</c>,
/// 再回主数据流按 id 过滤
/// </remarks>
public class OpenObserveAuditLogAction
{
    /// <summary>
    /// OpenObserve 文档时间(微秒)
    /// </summary>
    /// <remarks>
    /// 由 <see cref="AuditLogAction.ExecutionTime"/> 覆盖; 为 null 时使用服务端写入时间
    /// </remarks>
    [JsonPropertyName("_timestamp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? Timestamp { get; set; }

    public Guid Id { get; set; }

    public Guid AuditLogId { get; set; }

    public Guid? TenantId { get; set; }

    public string ServiceName { get; set; }

    public string MethodName { get; set; }

    public string Parameters { get; set; }

    public DateTime ExecutionTime { get; set; }

    public int ExecutionDuration { get; set; }

    /// <summary>
    /// 所属审计日志的用户(冗余, 便于独立检索)
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// 所属审计日志的用户名(冗余, 便于独立检索)
    /// </summary>
    public string? UserName { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; set; }

    public OpenObserveAuditLogAction()
    {
        ServiceName = string.Empty;
        MethodName = string.Empty;
        Parameters = string.Empty;
        ExtraProperties = new ExtraPropertyDictionary();
    }

    /// <summary>
    /// 从审计日志与操作创建展平记录
    /// </summary>
    public static OpenObserveAuditLogAction From(AuditLog auditLog, AuditLogAction action)
    {
        return new OpenObserveAuditLogAction
        {
            Id = action.Id,
            AuditLogId = action.AuditLogId,
            TenantId = action.TenantId ?? auditLog.TenantId,
            ServiceName = action.ServiceName,
            MethodName = action.MethodName,
            Parameters = action.Parameters,
            ExecutionTime = action.ExecutionTime,
            ExecutionDuration = action.ExecutionDuration,
            UserId = auditLog.UserId,
            UserName = auditLog.UserName,
            ExtraProperties = action.ExtraProperties ?? new ExtraPropertyDictionary(),
        };
    }

    /// <summary>
    /// 还原为操作
    /// </summary>
    public virtual AuditLogAction ToAuditLogAction()
    {
        return new AuditLogAction
        {
            Id = Id,
            AuditLogId = AuditLogId,
            TenantId = TenantId,
            ServiceName = ServiceName,
            MethodName = MethodName,
            Parameters = Parameters,
            ExecutionTime = ExecutionTime,
            ExecutionDuration = ExecutionDuration,
            ExtraProperties = ExtraProperties ?? new ExtraPropertyDictionary(),
        };
    }

    /// <summary>
    /// 批量创建展平记录
    /// </summary>
    public static List<OpenObserveAuditLogAction> CreateMany(AuditLog auditLog)
    {
        return auditLog.Actions?
            .Select(action => From(auditLog, action))
            .ToList()
            ?? new List<OpenObserveAuditLogAction>();
    }
}
