using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.AIManagement.Workspaces.Dtos;
public class WorkspaceDefinitionRecordUpdateDto : WorkspaceDefinitionRecordCreateOrUpdateDto, IHasConcurrencyStamp
{
    [Required]
    [StringLength(40)]
    public string ConcurrencyStamp { get; set; }
}
