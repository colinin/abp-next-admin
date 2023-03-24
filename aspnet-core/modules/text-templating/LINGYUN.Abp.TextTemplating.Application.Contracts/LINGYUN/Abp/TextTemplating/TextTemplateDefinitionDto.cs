using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.TextTemplating;

public class TextTemplateDefinitionDto : IHasConcurrencyStamp
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string DefaultCultureName { get; set; }
    public bool IsInlineLocalized { get; set; }
    public bool IsLayout { get; set; }
    public string Layout { get; set; }
    public string LayoutName { get; set; }
    public bool IsStatic { get; set; }
    public string ConcurrencyStamp { get; set; }
    public string FormatedDisplayName { get; set; }
}
