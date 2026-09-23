using System.Collections.Generic;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.IdentityServer.ApiScopes;

public class ApiScopeCreateOrUpdateDto
{
    public bool Enabled { get; set; }

    [DynamicStringLength(typeof(ApiScopeConsts), nameof(ApiScopeConsts.DisplayNameMaxLength))]
    public string? DisplayName { get; set; }

    [DynamicStringLength(typeof(ApiScopeConsts), nameof(ApiScopeConsts.DescriptionMaxLength))]
    public string? Description { get; set; }

    public bool Required { get; set; }

    public bool Emphasize { get; set; }

    public bool ShowInDiscoveryDocument { get; set; }

    public List<ApiScopeClaimDto> UserClaims { get; set; }

    public List<ApiScopePropertyDto> Properties { get; set; }

    public ApiScopeCreateOrUpdateDto()
    {
        UserClaims = new List<ApiScopeClaimDto>();
        Properties = new List<ApiScopePropertyDto>();
    }
}
