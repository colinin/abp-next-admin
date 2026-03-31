using System;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.AIManagement.Chats;
public abstract class ChatMessageRecordEto : EntityEto<Guid>, IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string Workspace { get; set; }
    public Guid? ConversationId { get; set; }
}
