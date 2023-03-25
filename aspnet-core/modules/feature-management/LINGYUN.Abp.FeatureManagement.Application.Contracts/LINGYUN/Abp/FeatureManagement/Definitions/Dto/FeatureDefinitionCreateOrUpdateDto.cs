using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Data;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.FeatureManagement.Definitions;

public abstract class FeatureDefinitionCreateOrUpdateDto : IHasExtraProperties
{
    [Required]
    [DynamicStringLength(typeof(FeatureDefinitionRecordConsts), nameof(FeatureDefinitionRecordConsts.MaxDisplayNameLength))]
    public string DisplayName { get; set; }

    [DynamicStringLength(typeof(FeatureDefinitionRecordConsts), nameof(FeatureDefinitionRecordConsts.MaxNameLength))]
    public string ParentName { get; set; }

    [DynamicStringLength(typeof(FeatureDefinitionRecordConsts), nameof(FeatureDefinitionRecordConsts.MaxDescriptionLength))]
    public string Description { get; set; }

    [DynamicStringLength(typeof(FeatureDefinitionRecordConsts), nameof(FeatureDefinitionRecordConsts.MaxDefaultValueLength))]
    public string DefaultValue { get; set; }

    public bool IsVisibleToClients { get; set; }

    public bool IsAvailableToHost { get; set; }

    public List<string> AllowedProviders { get; set; } = new List<string>();

    [DynamicStringLength(typeof(FeatureDefinitionRecordConsts), nameof(FeatureDefinitionRecordConsts.MaxValueTypeLength))]
    public string ValueType { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; set; } = new ExtraPropertyDictionary();
}
