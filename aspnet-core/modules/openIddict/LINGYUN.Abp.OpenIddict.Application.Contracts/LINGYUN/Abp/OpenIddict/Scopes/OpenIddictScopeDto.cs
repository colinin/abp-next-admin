using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.OpenIddict.Scopes;

[Serializable]
public class OpenIddictScopeDto : ExtensibleAuditedEntityDto<Guid>
{
    public string Description { get; set; }

    public Dictionary<string, string> Descriptions { get; set; } = new Dictionary<string, string>();


    public string DisplayName { get; set; }

    public Dictionary<string, string> DisplayNames { get; set; } = new Dictionary<string, string>();

    public string Name { get; set; }

    public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

    public List<string> Resources { get; set; }
}
