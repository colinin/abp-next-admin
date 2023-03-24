using Volo.Abp.Data;

namespace LINGYUN.Abp.WebhooksManagement.Definitions.Dto;

public class WebhookDefinitionDto : IHasExtraProperties
{
    public string GroupName { get; set; }

    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string FormatedDisplayName { get; set; }

    public string Description { get; set; }

    public string FormatedDescription { get; set; }

    public bool IsEnabled { get; set; }

    public bool IsStatic { get; set; }

    public string RequiredFeatures { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; set; } = new ExtraPropertyDictionary();
}
