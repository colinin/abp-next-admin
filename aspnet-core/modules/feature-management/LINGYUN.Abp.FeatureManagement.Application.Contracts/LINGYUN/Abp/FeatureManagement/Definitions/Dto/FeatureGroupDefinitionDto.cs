using Volo.Abp.Data;

namespace LINGYUN.Abp.FeatureManagement.Definitions;

public class FeatureGroupDefinitionDto : IHasExtraProperties
{
    public string Name { get; set; } = default!;

    public string DisplayName { get; set; } = default!;

    public bool IsStatic { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; set; } = default!;
}
