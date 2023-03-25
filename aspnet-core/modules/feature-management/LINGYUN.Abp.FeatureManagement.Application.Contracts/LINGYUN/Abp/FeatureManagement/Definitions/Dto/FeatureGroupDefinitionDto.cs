using Volo.Abp.Data;

namespace LINGYUN.Abp.FeatureManagement.Definitions;

public class FeatureGroupDefinitionDto : IHasExtraProperties
{
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string FormatedDisplayName { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; set; }
}
