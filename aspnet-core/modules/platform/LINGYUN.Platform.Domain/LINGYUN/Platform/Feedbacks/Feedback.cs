using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Platform.Feedbacks;
public class Feedback : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual string Content { get; set; }
    public virtual string Category { get; protected set; }
    public virtual FeedbackStatus Status { get; protected set; }
    public virtual ICollection<FeedbackComment> Comments { get; protected set; }
    public virtual ICollection<FeedbackAttachment> Attachments { get; protected set; }
    protected Feedback()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();

        Comments = new Collection<FeedbackComment>();
        Attachments = new Collection<FeedbackAttachment>();
    }

    public Feedback(
        Guid id,
        string category,
        string content, 
        FeedbackStatus status, 
        Guid? tenantId = null)
        : base(id)
    {
        Category = Check.NotNullOrWhiteSpace(category, nameof(category), FeedbackConsts.MaxCategoryLength);
        Content = Check.NotNullOrWhiteSpace(content, nameof(content), FeedbackConsts.MaxContentLength);
        Status = status;
        TenantId = tenantId;

        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();

        Comments = new Collection<FeedbackComment>();
        Attachments = new Collection<FeedbackAttachment>();
    }

    public FeedbackAttachment AddAttachment(
        IGuidGenerator guidGenerator,
        string name,
        string url,
        long size)
    {
        if (FindAttachment(name) != null)
        {
            throw new BusinessException(PlatformErrorCodes.DuplicateFeedbackAttachment)
                .WithData("Name", name);
        }

        var attachment = new FeedbackAttachment(
            guidGenerator.Create(),
            Id,
            name,
            url,
            size,
            TenantId);

        Attachments.Add(attachment);

        return attachment;
    }

    public FeedbackAttachment FindAttachment(string name)
    {
        return Attachments.FirstOrDefault(x => x.Name == name);
    }

    public Feedback RemoveAttachment(string name)
    {
        Attachments.RemoveAll(x => x.Name == name);

        return this;
    }

    public FeedbackComment Progress(
        IGuidGenerator generator,
        string capacity,
        string content)
    {
        ValidateStatus();

        var comment = new FeedbackComment(
            generator.Create(),
            Id,
            capacity,
            content, 
            TenantId);

        Comments.Add(comment);

        Status = FeedbackStatus.InProgress;

        return comment;
    }

    public FeedbackComment Close(
        IGuidGenerator generator,
        string capacity,
        string content)
    {
        ValidateStatus();

        var comment = new FeedbackComment(
            generator.Create(),
            Id,
            capacity,
            content, 
            TenantId);

        Comments.Add(comment);

        Status = FeedbackStatus.Closed;

        return comment;
    }

    public FeedbackComment Resolve(
       IGuidGenerator generator,
       string capacity,
       string content)
    {
        ValidateStatus();

        var comment = new FeedbackComment(
            generator.Create(),
            Id,
            capacity,
            content,
            TenantId);

        Comments.Add(comment);

        Status = FeedbackStatus.Resolved;

        return comment;
    }

    public Feedback ValidateStatus()
    {
        if (Status == FeedbackStatus.Closed)
        {
            throw new FeedbackStatusException(PlatformErrorCodes.UnableFeedbackCommentInStatus, FeedbackStatus.Closed);
        }
        if (Status == FeedbackStatus.Resolved)
        {
            throw new FeedbackStatusException(PlatformErrorCodes.UnableFeedbackCommentInStatus, FeedbackStatus.Resolved);
        }
        return this;
    }
}
