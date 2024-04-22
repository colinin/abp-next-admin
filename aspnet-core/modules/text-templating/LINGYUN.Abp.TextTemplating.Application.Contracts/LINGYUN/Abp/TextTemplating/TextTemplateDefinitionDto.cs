using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;

namespace LINGYUN.Abp.TextTemplating;

public class TextTemplateDefinitionDto : ExtensibleObject, IHasConcurrencyStamp
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string DefaultCultureName { get; set; }
    public string LocalizationResourceName { get; set; }
    public string RenderEngine { get; set; }
    public bool IsInlineLocalized { get; set; }
    public bool IsLayout { get; set; }
    public string Layout { get; set; }
    public bool IsStatic { get; set; }
    public string ConcurrencyStamp { get; set; }
}
