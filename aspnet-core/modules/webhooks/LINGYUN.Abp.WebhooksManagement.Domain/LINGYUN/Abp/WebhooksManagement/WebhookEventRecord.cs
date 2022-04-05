using System;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.WebhooksManagement;

public class WebhookEventRecord : Entity<Guid>, IHasCreationTime, IHasDeletionTime
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
        WebhookName = Check.NotNullOrWhiteSpace(webhookName, nameof(webhookName), WebhookEventRecordConsts.MaxWebhookNameLength);
        Data = Check.Length(data, nameof(data), WebhookEventRecordConsts.MaxDataLength);
        TenantId = tenantId;
    }
}
