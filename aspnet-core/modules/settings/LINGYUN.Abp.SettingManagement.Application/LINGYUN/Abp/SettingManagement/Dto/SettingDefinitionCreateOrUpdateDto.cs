using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Data;
using Volo.Abp.SettingManagement;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.SettingManagement;

public abstract class SettingDefinitionCreateOrUpdateDto : IHasExtraProperties
{
    [Required]
    [DynamicStringLength(typeof(SettingDefinitionRecordConsts), nameof(SettingDefinitionRecordConsts.MaxDisplayNameLength))]
    public string DisplayName { get; set; }

    [DynamicStringLength(typeof(SettingDefinitionRecordConsts), nameof(SettingDefinitionRecordConsts.MaxDescriptionLength))]
    public string Description { get; set; }

    [DynamicStringLength(typeof(SettingDefinitionRecordConsts), nameof(SettingDefinitionRecordConsts.MaxDefaultValueLength))]
    public string DefaultValue { get; set; }

    public bool IsVisibleToClients { get; set; }

    public List<string> Providers { get; set; }

    public bool IsInherited { get; set; }

    public bool IsEncrypted { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; set; }

    protected SettingDefinitionCreateOrUpdateDto()
    {
        ExtraProperties = new ExtraPropertyDictionary();
    }
}
