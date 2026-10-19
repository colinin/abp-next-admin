using System;
using System.Text.Json.Serialization;
using Volo.Abp.Data;

namespace LINGYUN.Abp.AuditLogging.OpenObserve;

/// <summary>
/// 展平的实体变更记录(写入 OpenObserve 实体变更数据流)
/// </summary>
/// <remarks>
/// 纯数据对象, 不包含序列化逻辑(见 <see cref="OpenObserveEntityChangeConverter"/>);
/// 因为 OpenObserve 会把嵌套数组保存为 JSON 字符串列,
/// <see cref="EntityChange.PropertyChanges"/> 以 JSON 字符串形式保存
/// </remarks>
public class OpenObserveEntityChange
{
    /// <summary>
    /// OpenObserve 文档时间(微秒)
    /// </summary>
    /// <remarks>
    /// 由 <see cref="EntityChange.ChangeTime"/> 覆盖; 为 null 时使用服务端写入时间
    /// </remarks>
    [JsonPropertyName("_timestamp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? Timestamp { get; set; }

    public Guid Id { get; set; }

    public Guid AuditLogId { get; set; }

    public Guid? TenantId { get; set; }

    public Guid? EntityTenantId { get; set; }

    public DateTime ChangeTime { get; set; }

    /// <summary>
    /// 变更类型(<see cref="EntityChangeType"/> 的名称)
    /// </summary>
    public string ChangeType { get; set; }

    public string? EntityId { get; set; }

    public string? EntityTypeFullName { get; set; }

    /// <summary>
    /// 审计日志所属用户
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// 审计日志所属用户名
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 属性变更明细(JSON 字符串, 由 <see cref="OpenObserveEntityChangeConverter"/> 处理)
    /// </summary>
    [JsonPropertyName("propertychanges")]
    public string? PropertyChangesJson { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; set; }

    public OpenObserveEntityChange()
    {
        ChangeType = string.Empty;
        ExtraProperties = new ExtraPropertyDictionary();
    }
}
