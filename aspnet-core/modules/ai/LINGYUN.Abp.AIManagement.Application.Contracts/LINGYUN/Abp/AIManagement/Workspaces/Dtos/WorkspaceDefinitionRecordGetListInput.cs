using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.AIManagement.Workspaces.Dtos;

[Serializable]
public class WorkspaceDefinitionRecordGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    public string? Provider { get; set; }

    public string? ModelName { get; set; }
}
