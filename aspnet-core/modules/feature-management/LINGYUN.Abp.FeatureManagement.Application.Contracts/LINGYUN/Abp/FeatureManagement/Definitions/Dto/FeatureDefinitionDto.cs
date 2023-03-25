using System.Collections.Generic;
using Volo.Abp.Data;

namespace LINGYUN.Abp.FeatureManagement.Definitions;

public class FeatureDefinitionDto : IHasExtraProperties
{
    public string GroupName { get; set; }

    public string Name { get; set; }

    public string ParentName { get; set; }

    public string DisplayName { get; set; }

    public string FormatedDisplayName { get; set; }

    public string Description { get; set; }

    public string FormatedDescription { get; set; }

    public string DefaultValue { get; set; }

    public bool IsVisibleToClients { get; set; }

    public bool IsAvailableToHost { get; set; }

    public List<string> AllowedProviders { get; set; } = new List<string>();

    public string ValueType { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; set; } = new ExtraPropertyDictionary();
}
