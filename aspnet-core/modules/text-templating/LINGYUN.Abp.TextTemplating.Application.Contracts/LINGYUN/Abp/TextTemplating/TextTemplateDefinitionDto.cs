namespace LINGYUN.Abp.TextTemplating;

public class TextTemplateDefinitionDto
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string DefaultCultureName { get; set; }
    public bool IsInlineLocalized { get; set; }
    public bool IsLayout { get; set; }
    public string Layout { get; set; }
}
