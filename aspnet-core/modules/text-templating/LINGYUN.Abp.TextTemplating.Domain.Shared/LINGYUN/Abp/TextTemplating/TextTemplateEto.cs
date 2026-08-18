using System;

namespace LINGYUN.Abp.TextTemplating;

[Serializable]
public class TextTemplateEto 
{
    public string Name { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string? Content { get; set; }
    public string? Culture { get; set; }
}
