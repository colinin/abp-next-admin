using System;

namespace LINGYUN.Abp.PrintDesigner.CustomElements.Dtos;

public class CustomElementUpsertDto : CustomElementCreateOrUpdateDto
{
    public Guid? Id { get; set; }

    public string? ConcurrencyStamp { get; set; }
}
