using System.ComponentModel.DataAnnotations;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.FeatureManagement.Definitions;

public class FeatureDefinitionCreateDto : FeatureDefinitionCreateOrUpdateDto
{
    [Required]
    [DynamicStringLength(typeof(FeatureDefinitionRecordConsts), nameof(FeatureDefinitionRecordConsts.MaxNameLength))]
    public string Name { get; set; }

    [Required]
    [DynamicStringLength(typeof(FeatureGroupDefinitionRecordConsts), nameof(FeatureGroupDefinitionRecordConsts.MaxNameLength))]
    public string GroupName { get; set; }
}
