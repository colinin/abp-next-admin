using System.Collections.Generic;
using System.Text.Json.Serialization;
using Volo.Abp.Data;

namespace LINGYUN.Abp.AuditLogging.OpenObserve;

/// <summary>
/// 写入 OpenObserve 的审计日志文档
/// </summary>
/// <remarks>
/// 继承 <see cref="AuditLog"/> 并追加 <c>_timestamp</c> 列(由 <see cref="AuditLog.ExecutionTime"/> 覆盖);
/// 实体变更与操作写入由 AuditLogStream 计算出的独立数据流
/// </remarks>
public class OpenObserveAuditLog : AuditLog
{
    /// <summary>
    /// OpenObserve 文档时间(微秒)
    /// </summary>
    /// <remarks>
    /// 由 <see cref="AuditLog.ExecutionTime"/> 覆盖; 为 null 时使用服务端写入时间
    /// </remarks>
    [JsonPropertyName("_timestamp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? Timestamp { get; set; }

    public static OpenObserveAuditLog From(AuditLog auditLog)
    {
        return new OpenObserveAuditLog
        {
            Id = auditLog.Id,
            ApplicationName = auditLog.ApplicationName,
            UserId = auditLog.UserId,
            UserName = auditLog.UserName,
            TenantId = auditLog.TenantId,
            TenantName = auditLog.TenantName,
            ImpersonatorUserId = auditLog.ImpersonatorUserId,
            ImpersonatorUserName = auditLog.ImpersonatorUserName,
            ImpersonatorTenantId = auditLog.ImpersonatorTenantId,
            ImpersonatorTenantName = auditLog.ImpersonatorTenantName,
            ExecutionTime = auditLog.ExecutionTime,
            ExecutionDuration = auditLog.ExecutionDuration,
            ClientIpAddress = auditLog.ClientIpAddress,
            ClientName = auditLog.ClientName,
            ClientId = auditLog.ClientId,
            CorrelationId = auditLog.CorrelationId,
            BrowserInfo = auditLog.BrowserInfo,
            HttpMethod = auditLog.HttpMethod,
            Url = auditLog.Url,
            Exceptions = auditLog.Exceptions,
            Comments = auditLog.Comments,
            HttpStatusCode = auditLog.HttpStatusCode,
            // 实体变更与操作写入独立数据流(见 GetEntityChangeStream / GetActionStream), 主文档不再冗余保存, 详情按需从子流获取
            EntityChanges = new List<EntityChange>(),
            Actions = new List<AuditLogAction>(),
            ExtraProperties = auditLog.ExtraProperties ?? new ExtraPropertyDictionary(),
            // _timestamp 由写入器按 AuditLog.ExecutionTime 覆盖(见 OpenObserveTimestampHelper)
            Timestamp = null,
        };
    }
}
