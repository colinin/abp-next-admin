using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.AIManagement.Chats;
public class ConversationRecord : AuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; private set; }

    public string Name { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime ExpiredAt { get; set; }

    public DateTime? UpdateAt { get; set; }
    public ConversationRecord(
        Guid id,
        string name,
        DateTime createdAt,
        DateTime expiredAt,
        Guid? tenantId = null)
        : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), ConversationRecordConsts.MaxNameLength);
        CreatedAt = createdAt;
        ExpiredAt = expiredAt;

        UpdateAt = createdAt;

        TenantId = tenantId;
    }

    public void SetName(string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), ConversationRecordConsts.MaxNameLength);
    }
}
