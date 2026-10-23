using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates.Dtos;

public class TemplateSaveDesignDto
{
    [Required]
    public string FlowDefinitionJson { get; set; } = "{}";

    public Guid? FormDefinitionId { get; set; }
}
