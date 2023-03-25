using System.ComponentModel.DataAnnotations;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.FeatureManagement.Definitions;
public class FeatureGroupDefinitionCreateDto : FeatureGroupDefinitionCreateOrUpdateDto
{
    [Required]
    [DynamicStringLength(typeof(FeatureGroupDefinitionRecordConsts), nameof(FeatureGroupDefinitionRecordConsts.MaxNameLength))]
    public string Name { get; set; }
}
