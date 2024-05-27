using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace LINGYUN.Abp.TextTemplating;

public class TextTemplate : AuditedEntity<Guid>
{
    public virtual string Name { get; private set; }
    public virtual string DisplayName { get; private set; }
    public virtual string Content { get; private set; }
    public virtual string Culture { get; private set; }
    protected TextTemplate() { }
    public TextTemplate(
        Guid id,
        string name,
        string displayName,
        string content,
        string culture = null)
        : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), TextTemplateConsts.MaxNameLength);
        DisplayName = Check.NotNullOrWhiteSpace(displayName, nameof(displayName), TextTemplateConsts.MaxDisplayNameLength);
        Content = Check.NotNullOrWhiteSpace(content, nameof(content), TextTemplateConsts.MaxContentLength);
        Culture = Check.Length(culture, nameof(culture), TextTemplateConsts.MaxCultureLength);
    }

    public void SetContent(string content)
    {
        Content = Check.NotNullOrWhiteSpace(content, nameof(content), TextTemplateConsts.MaxContentLength);
    }
}
