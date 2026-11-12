namespace LINGYUN.Abp.PrintDesigner.Templates.Dtos;

public class TemplateUpdateDto : TemplateCreateOrUpdateDto
{
    public string? ConcurrencyStamp { get; set; }
}
