using LINGYUN.Abp.AIManagement.Workspaces;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.AIManagement.Chats;
public class ConversationRecord : AuditedEntity<Guid>, IMultiTenant
{
    public Guid? TenantId { get; private set; }

    public string Name { get; private set; }

    public string Workspace { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime ExpiredAt { get; set; }

    public DateTime? UpdateAt { get; set; }
    public ConversationRecord(
        Guid id,
        string name,
        string workspace,
        DateTime createdAt,
        DateTime expiredAt,
        Guid? tenantId = null)
        : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), ConversationRecordConsts.MaxNameLength);
        Workspace = Check.NotNullOrWhiteSpace(workspace, nameof(workspace), WorkspaceDefinitionRecordConsts.MaxNameLength);
        CreatedAt = createdAt;
        CreationTime = createdAt;
        ExpiredAt = expiredAt;

        UpdateAt = createdAt;

        TenantId = tenantId;
    }

    public void SetName(string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), ConversationRecordConsts.MaxNameLength);
    }

    public void ChangeTime(DateTime updateTime)
    {
        UpdateAt = updateTime;
        LastModificationTime = updateTime;
    }
}
