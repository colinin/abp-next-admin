namespace LINGYUN.Abp.PrintDesigner.CustomElements.Dtos;

public class CustomElementUpdateDto : CustomElementCreateOrUpdateDto
{
    public string? ConcurrencyStamp { get; set; }
}
