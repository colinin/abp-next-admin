namespace LINGYUN.Abp.TextTemplating;

public class TextTemplateDefinitionGetListInput
{
    public string Filter { get; set; }
    public bool? IsStatic { get; set; }
    public bool? IsLayout { get; set; }
}
