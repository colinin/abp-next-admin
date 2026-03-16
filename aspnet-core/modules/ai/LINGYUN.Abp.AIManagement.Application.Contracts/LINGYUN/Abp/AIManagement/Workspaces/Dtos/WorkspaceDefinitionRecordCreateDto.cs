using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.AIManagement.Workspaces.Dtos;
public class WorkspaceDefinitionRecordCreateDto : WorkspaceDefinitionRecordCreateOrUpdateDto
{
    [Required]
    [DynamicStringLength(typeof(WorkspaceDefinitionRecordConsts), nameof(WorkspaceDefinitionRecordConsts.MaxNameLength))]
    public string Name { get; set; }
}
