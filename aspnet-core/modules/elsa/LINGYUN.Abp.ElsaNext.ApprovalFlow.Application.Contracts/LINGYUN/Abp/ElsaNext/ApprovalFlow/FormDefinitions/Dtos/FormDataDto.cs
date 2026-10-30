using System;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions.Dtos;

public class FormDataDto
{
    public Guid Id { get; set; }
    public Guid FormDefinitionId { get; set; }
    public string FormCode { get; set; } = "";
    public int FormVersion { get; set; }
    public string InstanceId { get; set; } = "";
    public string DataJson { get; set; } = "{}";
}
