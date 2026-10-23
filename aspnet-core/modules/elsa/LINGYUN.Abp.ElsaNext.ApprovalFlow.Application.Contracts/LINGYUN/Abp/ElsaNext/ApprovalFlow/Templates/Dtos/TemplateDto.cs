using System;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates.Dtos;

public class TemplateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Icon { get; set; }
    public string? BusinessKeyHint { get; set; }
    public bool IsPublished { get; set; }
    public bool IsEnabled { get; set; }
    public string? ElsaDefinitionId { get; set; }
    public string? ElsaDefinitionKey { get; set; }
    public int? ElsaDefinitionVersion { get; set; }
    public Guid? FormDefinitionId { get; set; }
    public string? FlowDefinitionJson { get; set; }
}
