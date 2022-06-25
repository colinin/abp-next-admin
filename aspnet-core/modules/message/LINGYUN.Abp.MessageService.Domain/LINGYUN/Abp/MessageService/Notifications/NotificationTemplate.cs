using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MessageService.Notifications;

public class NotificationTemplate : AuditedEntity<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual string Name { get; private set; }
    public virtual string Description { get; private set; }
    public virtual string Title { get; private set; }
    public virtual string Content { get; private set; }
    public virtual string Culture { get; private set; }
    protected NotificationTemplate() { }
    public NotificationTemplate(
        Guid id,
        string name,
        string title,
        string content,
        string culture,
        string description = null)
        : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), NotificationTemplateConsts.MaxNameLength);
        Title = Check.NotNullOrWhiteSpace(title, nameof(title), NotificationTemplateConsts.MaxTitleLength);
        Content = Check.NotNullOrWhiteSpace(content, nameof(content), NotificationTemplateConsts.MaxContentLength);
        Culture = Check.NotNullOrWhiteSpace(culture, nameof(culture), NotificationTemplateConsts.MaxCultureLength);
        Description = Check.Length(description, nameof(description), NotificationTemplateConsts.MaxDescriptionLength);
    }

    public void SetContent(string content)
    {
        Content = Check.NotNullOrWhiteSpace(content, nameof(content), NotificationTemplateConsts.MaxContentLength);
    }
}
