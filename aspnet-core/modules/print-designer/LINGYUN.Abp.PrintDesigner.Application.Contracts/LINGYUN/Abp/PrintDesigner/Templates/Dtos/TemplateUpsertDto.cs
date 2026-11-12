using System;

namespace LINGYUN.Abp.PrintDesigner.Templates.Dtos;

public class TemplateUpsertDto : TemplateCreateOrUpdateDto
{
    public Guid? Id { get; set; }

    public string? ConcurrencyStamp { get; set; }
}
