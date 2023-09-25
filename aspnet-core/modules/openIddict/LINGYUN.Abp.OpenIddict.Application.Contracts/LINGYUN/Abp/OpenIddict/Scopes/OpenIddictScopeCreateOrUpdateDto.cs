using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.OpenIddict.Scopes;

public abstract class OpenIddictScopeCreateOrUpdateDto : ExtensibleObject
{

    public string Description { get; set; }

    public Dictionary<string, string> Descriptions { get; set; } = new Dictionary<string, string>();

    public string DisplayName { get; set; }

    public Dictionary<string, string> DisplayNames { get; set; } = new Dictionary<string, string>();

    [Required]
    [DynamicStringLength(typeof(OpenIddictScopeConsts), nameof(OpenIddictScopeConsts.NameMaxLength))]
    public string Name { get; set; }

    public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

    public List<string> Resources { get; set; } = new List<string>();
}
