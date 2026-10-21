using LINGYUN.Abp.AuditLogging.OpenObserve.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.AuditLogging.OpenObserve;

/// <summary>
/// 实体变更文档转换器
/// </summary>
/// <remarks>
/// OpenObserve 会把嵌套数组保存为 JSON 字符串列, 因此实体变更的属性明细
/// (<c>propertychanges</c>) 在写入前序列化、读取后反序列化;
/// 转换逻辑集中在此处以保持实体模型为纯数据对象
/// </remarks>
public class OpenObserveEntityChangeConverter : ITransientDependency
{
    protected IAuditLoggingOpenObserveSerializer Serializer { get; }

    public OpenObserveEntityChangeConverter(IAuditLoggingOpenObserveSerializer serializer)
    {
        Serializer = serializer;
    }

    /// <summary>
    /// 审计日志 -> 实体变更文档集合
    /// </summary>
    public virtual List<OpenObserveEntityChange> ToDocuments(AuditLog auditLog)
    {
        return auditLog.EntityChanges?
            .Select(entityChange => ToDocument(auditLog, entityChange))
            .ToList()
            ?? new List<OpenObserveEntityChange>();
    }

    /// <summary>
    /// 实体变更 -> 实体变更文档
    /// </summary>
    public virtual OpenObserveEntityChange ToDocument(AuditLog auditLog, EntityChange entityChange)
    {
        return new OpenObserveEntityChange
        {
            Id = entityChange.Id,
            AuditLogId = entityChange.AuditLogId,
            TenantId = entityChange.TenantId,
            EntityTenantId = entityChange.EntityTenantId,
            ChangeTime = entityChange.ChangeTime,
            ChangeType = entityChange.ChangeType.ToString(),
            EntityId = entityChange.EntityId,
            EntityTypeFullName = entityChange.EntityTypeFullName,
            UserId = auditLog.UserId,
            UserName = auditLog.UserName,
            PropertyChangesJson = SerializePropertyChanges(entityChange.PropertyChanges),
            ExtraProperties = entityChange.ExtraProperties ?? new ExtraPropertyDictionary(),
        };
    }

    /// <summary>
    /// 实体变更文档 -> 实体变更
    /// </summary>
    public virtual EntityChange ToEntityChange(OpenObserveEntityChange document)
    {
        return new EntityChange
        {
            Id = document.Id,
            AuditLogId = document.AuditLogId,
            TenantId = document.TenantId,
            EntityTenantId = document.EntityTenantId,
            ChangeTime = document.ChangeTime,
            ChangeType = ParseChangeType(document.ChangeType),
            EntityId = document.EntityId,
            EntityTypeFullName = document.EntityTypeFullName,
            PropertyChanges = DeserializePropertyChanges(document.PropertyChangesJson),
            ExtraProperties = document.ExtraProperties ?? new ExtraPropertyDictionary(),
        };
    }

    /// <summary>
    /// 实体变更文档 -> 附带用户名的实体变更
    /// </summary>
    public virtual EntityChangeWithUsername ToEntityChangeWithUsername(OpenObserveEntityChange document)
    {
        return new EntityChangeWithUsername
        {
            UserName = document.UserName,
            EntityChange = ToEntityChange(document),
        };
    }

    /// <summary>
    /// 序列化属性变更明细
    /// </summary>
    protected virtual string? SerializePropertyChanges(IEnumerable<EntityPropertyChange>? propertyChanges)
    {
        var changes = propertyChanges?.ToList();

        return changes == null || changes.Count == 0
            ? null
            : Serializer.Serialize(changes);
    }

    /// <summary>
    /// 反序列化属性变更明细
    /// </summary>
    protected virtual List<EntityPropertyChange> DeserializePropertyChanges(string? propertyChangesJson)
    {
        return Serializer.Deserialize<List<EntityPropertyChange>>(propertyChangesJson ?? string.Empty)
            ?? new List<EntityPropertyChange>();
    }

    protected virtual EntityChangeType ParseChangeType(string changeType)
    {
        return Enum.TryParse<EntityChangeType>(changeType, ignoreCase: true, out var parsed)
            ? parsed
            : EntityChangeType.Updated;
    }
}
