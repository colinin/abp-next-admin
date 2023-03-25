using System.ComponentModel.DataAnnotations;
using Volo.Abp.Data;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.FeatureManagement.Definitions;

public abstract class FeatureGroupDefinitionCreateOrUpdateDto : IHasExtraProperties
{
    [Required]
    [DynamicStringLength(typeof(FeatureGroupDefinitionRecordConsts), nameof(FeatureGroupDefinitionRecordConsts.MaxDisplayNameLength))]
    public string DisplayName { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; set; } = new ExtraPropertyDictionary();
}
