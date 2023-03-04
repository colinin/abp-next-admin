using System;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.TextTemplating;
public class TextTemplateDefinition : AggregateRoot<Guid>, IHasExtraProperties
{
    public virtual string Name { get; protected set; }
    public virtual string DisplayName { get; set; }
    public virtual bool IsLayout { get; set; }
    public virtual string Layout { get; set; }
    public virtual bool IsInlineLocalized { get; set; }
    public virtual string DefaultCultureName { get; set; }
    public virtual string RenderEngine { get; set; }
    public virtual bool IsStatic { get; set; }
    protected TextTemplateDefinition()
    {

    }

    public TextTemplateDefinition(
        Guid id,
        string name,
        string displayName,
        bool isLayout = false,
        string layout = null,
        bool isInlineLocalized = false, 
        string defaultCultureName = null, 
        string renderEngine = null)
        : base(id)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), TextTemplateDefinitionConsts.MaxNameLength);
        DisplayName = Check.NotNullOrWhiteSpace(displayName, nameof(displayName), TextTemplateDefinitionConsts.MaxDisplayNameLength);
        IsLayout = isLayout;
        Layout = Check.Length(layout, nameof(layout), TextTemplateDefinitionConsts.MaxLayoutLength);
        IsInlineLocalized = isInlineLocalized;
        DefaultCultureName = Check.Length(defaultCultureName, nameof(defaultCultureName), TextTemplateDefinitionConsts.MaxDefaultCultureNameLength);
        RenderEngine = Check.Length(renderEngine, nameof(renderEngine), TextTemplateDefinitionConsts.MaxRenderEngineLength);
    }
}
