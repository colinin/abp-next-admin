using System;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;

namespace LINGYUN.Abp.DataProtectionManagement;

[Serializable]
[EventName("abp.data_protection.entity_type_info")]
public class EntityTypeInfoEto : EntityEto<Guid>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 显示名称
    /// </summary>
    public string DisplayName { get; set; }
    /// <summary>
    /// 类型全名
    /// </summary>
    public string TypeFullName { get; set; }
    /// <summary>
    /// 是否启用数据审计
    /// </summary>
    public bool IsAuditEnabled { get; set; }
}
