using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Platform.Feedbacks;
public class FeedbackAttachment : CreationAuditedEntity<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual string Name { get; protected set; }
    public virtual string Url { get; protected set; }
    public virtual long Size { get; protected set; }
    public virtual Guid FeedbackId { get; protected set; }
    protected FeedbackAttachment()
    {

    }

    internal FeedbackAttachment(
        Guid id,
        Guid feedbackId,
        string name,
        string url, 
        long size, 
        Guid? tenantId)
        :base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), FeedbackAttachmentConsts.MaxNameLength);
        Url = Check.NotNullOrWhiteSpace(url, nameof(url), FeedbackAttachmentConsts.MaxUrlLength);
        Size = size;
        FeedbackId = feedbackId;
        TenantId = tenantId;
    }
}
