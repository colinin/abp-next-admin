using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.AIManagement.Tools.Dtos;

[Serializable]
public class AIToolDefinitionRecordGetPagedListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    public bool? IsEnabled { get; set; }

    public string? Provider { get; set; }
}
