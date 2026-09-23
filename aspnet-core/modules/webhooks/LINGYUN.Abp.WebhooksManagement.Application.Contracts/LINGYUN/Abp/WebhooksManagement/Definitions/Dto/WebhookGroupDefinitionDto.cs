using Volo.Abp.Data;

namespace LINGYUN.Abp.WebhooksManagement.Definitions;

public class WebhookGroupDefinitionDto : IHasExtraProperties
{
    public string Name { get; set; } = default!;

    public string DisplayName { get; set; } = default!;

    public bool IsStatic { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; set; } = new ExtraPropertyDictionary();
}
