using System.Collections.Generic;
using Volo.Abp.Data;

namespace LINGYUN.Abp.WebhooksManagement.Definitions.Dto;

public class WebhookDefinitionDto : IHasExtraProperties
{
    public string GroupName { get; set; }

    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public bool IsEnabled { get; set; }

    public bool IsStatic { get; set; }

    public List<string> RequiredFeatures { get; set; } = new();

    public ExtraPropertyDictionary ExtraProperties { get; set; } = new ExtraPropertyDictionary();
}
