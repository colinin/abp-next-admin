using System;

namespace LINGYUN.Abp.TextTemplating;

[Serializable]
public class TextTemplateDefinitionEto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public bool IsLayout { get; set; }
    public string? Layout { get; set; }
    public bool IsInlineLocalized { get; set; }
    public string? DefaultCultureName { get; set; }
    public string? RenderEngine { get; set; }
    public bool IsStatic { get; set; }
}
