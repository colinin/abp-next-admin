using System.Collections.Generic;
using Volo.Abp.Auditing;
using Volo.Abp.ObjectExtending;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.OpenIddict.Applications;

public abstract class OpenIddictApplicationCreateOrUpdateDto : ExtensibleObject
{
    [DisableAuditing]
    public string ClientSecret { get; set; }

    [DynamicStringLength(typeof(OpenIddictApplicationConsts), nameof(OpenIddictApplicationConsts.ConsentTypeMaxLength))]
    public string ConsentType { get; set; }

    public string DisplayName { get; set; }

    public Dictionary<string, string> DisplayNames { get; set; } = new Dictionary<string, string>();

    public List<string> Endpoints { get; set; } = new List<string>();
    public List<string> GrantTypes { get; set; } = new List<string>();
    public List<string> ResponseTypes { get; set; } = new List<string>();
    public List<string> Scopes { get; set; } = new List<string>();

    public List<string> PostLogoutRedirectUris { get; set; } = new List<string>();

    public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

    public List<string> RedirectUris { get; set; } = new List<string>();

    public List<string> Requirements { get; set; } = new List<string>();

    [DynamicStringLength(typeof(OpenIddictApplicationConsts), nameof(OpenIddictApplicationConsts.TypeMaxLength))]
    public string Type { get; set; }

    public string ClientUri { get; set; }

    public string LogoUri { get; set; }
}
