using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances.Dtos;

public class InstanceGetPagedListInput : PagedAndSortedResultRequestDto
{
    public Guid? TemplateId { get; set; }

    public string? Status { get; set; }

    public string? Filter { get; set; }

    public bool OnlyMine { get; set; } = true;
}
