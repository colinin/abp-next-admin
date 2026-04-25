using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.AIManagement.Tools.Dtos;
public class AIToolDefinitionRecordCreateDto : AIToolDefinitionRecordCreateOrUpdateDto
{
    [Required]
    [DynamicStringLength(typeof(AIToolDefinitionRecordConsts), nameof(AIToolDefinitionRecordConsts.MaxNameLength))]
    public string Name { get; set; }
}
