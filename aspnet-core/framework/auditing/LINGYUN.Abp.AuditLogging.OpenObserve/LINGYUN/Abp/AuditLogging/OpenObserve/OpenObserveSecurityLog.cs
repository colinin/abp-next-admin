using System;
using System.Text.Json.Serialization;
using Volo.Abp.Data;
using Volo.Abp.SecurityLog;

namespace LINGYUN.Abp.AuditLogging.OpenObserve;

/// <summary>
/// 写入 OpenObserve 的安全日志文档
/// </summary>
/// <remarks>
/// 继承 <see cref="SecurityLog"/> 并追加 <c>_timestamp</c> 列(由 <see cref="SecurityLog.CreationTime"/> 覆盖)
/// </remarks>
public class OpenObserveSecurityLog : SecurityLog
{
    /// <summary>
    /// OpenObserve 文档时间(微秒)
    /// </summary>
    /// <remarks>
    /// 由 <see cref="SecurityLog.CreationTime"/> 覆盖; 为 null 时使用服务端写入时间
    /// </remarks>
    [JsonPropertyName("_timestamp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? Timestamp { get; set; }

    public static OpenObserveSecurityLog From(SecurityLog securityLog)
    {
        return new OpenObserveSecurityLog
        {
            Id = securityLog.Id,
            TenantId = securityLog.TenantId,
            TenantName = securityLog.TenantName,
            ApplicationName = securityLog.ApplicationName,
            Identity = securityLog.Identity,
            Action = securityLog.Action,
            UserId = securityLog.UserId,
            UserName = securityLog.UserName,
            ClientId = securityLog.ClientId,
            CorrelationId = securityLog.CorrelationId,
            ClientIpAddress = securityLog.ClientIpAddress,
            BrowserInfo = securityLog.BrowserInfo,
            CreationTime = securityLog.CreationTime,
            ExtraProperties = securityLog.ExtraProperties ?? new ExtraPropertyDictionary(),
            Timestamp = null,
        };
    }
}
