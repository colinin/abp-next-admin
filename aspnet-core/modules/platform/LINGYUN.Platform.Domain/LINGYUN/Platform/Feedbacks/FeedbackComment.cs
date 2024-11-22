using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Platform.Feedbacks;
/// <summary>
/// 评论
/// </summary>
public class FeedbackComment : AuditedEntity<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual string Capacity { get; protected set; }
    public virtual string Content { get; set; }
    public virtual Guid FeedbackId { get; protected set; }
    protected FeedbackComment()
    {

    }

    internal FeedbackComment(
        Guid id,
        Guid feedbackId,
        string capacity,
        string content, 
        Guid? tenantId = null)
        : base(id)
    {
        Capacity = Check.NotNullOrWhiteSpace(capacity, nameof(capacity), FeedbackCommentConsts.MaxCapacityLength);
        Content = Check.NotNullOrWhiteSpace(content, nameof(content), FeedbackCommentConsts.MaxContentLength);
        FeedbackId = feedbackId;
        TenantId = tenantId;
    }
}
