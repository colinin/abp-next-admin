using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.AIManagement.Tools.Dtos;
public class AIToolDefinitionRecordUpdateDto : AIToolDefinitionRecordCreateOrUpdateDto, IHasConcurrencyStamp
{
    [Required]
    [StringLength(40)]
    public string ConcurrencyStamp { get; set; }
}
