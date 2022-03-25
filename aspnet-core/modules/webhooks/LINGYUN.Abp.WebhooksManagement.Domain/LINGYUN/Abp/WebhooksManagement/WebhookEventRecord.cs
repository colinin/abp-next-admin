using System;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.WebhooksManagement;

public class WebhookEventRecord : Entity<Guid>, IMultiTenant, IHasCreationTime, IHasDeletionTime
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual string WebhookName { get; protected set; }
    public virtual string Data { get; protected set; }
    public virtual DateTime CreationTime { get; set; }
    public virtual DateTime? DeletionTime { get; set; }
    public virtual bool IsDeleted { get; set; }
    protected WebhookEventRecord()
    {
    }

    public WebhookEventRecord(
        Guid id,
        string webhookName,
        string data,
        Guid? tenantId = null) : base(id)
    {
        WebhookName = webhookName;
        Data = data;
        TenantId = tenantId;
    }
}
