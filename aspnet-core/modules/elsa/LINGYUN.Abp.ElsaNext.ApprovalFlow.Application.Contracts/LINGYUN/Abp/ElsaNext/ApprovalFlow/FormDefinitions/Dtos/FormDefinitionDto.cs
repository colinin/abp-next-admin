using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions.Dtos;

public class FormDefinitionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Code { get; set; } = "";
    public int Version { get; set; }
    public string? Description { get; set; }
    public bool IsPublished { get; set; }
    public DateTimeOffset? PublishedTime { get; set; }
    public List<FormFieldDto> Fields { get; set; } = [];
}
