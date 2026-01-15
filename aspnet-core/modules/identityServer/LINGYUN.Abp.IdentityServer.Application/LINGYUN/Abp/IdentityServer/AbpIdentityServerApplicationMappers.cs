using LINGYUN.Abp.IdentityServer.ApiResources;
using LINGYUN.Abp.IdentityServer.ApiScopes;
using LINGYUN.Abp.IdentityServer.Clients;
using LINGYUN.Abp.IdentityServer.Grants;
using LINGYUN.Abp.IdentityServer.IdentityResources;
using Riok.Mapperly.Abstractions;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Grants;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.IdentityServer;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ClientSecretToClientSecretDtoMapper : MapperBase<ClientSecret, ClientSecretDto>
{
    public override partial ClientSecretDto Map(ClientSecret source);
    public override partial void Map(ClientSecret source, ClientSecretDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ClientScopeToClientScopeDtoMapper : MapperBase<ClientScope, ClientScopeDto>
{
    public override partial ClientScopeDto Map(ClientScope source);
    public override partial void Map(ClientScope source, ClientScopeDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ClientGrantTypeToClientGrantTypeDtoMapper : MapperBase<ClientGrantType, ClientGrantTypeDto>
{
    public override partial ClientGrantTypeDto Map(ClientGrantType source);
    public override partial void Map(ClientGrantType source, ClientGrantTypeDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ClientCorsOriginToClientCorsOriginDtoMapper : MapperBase<ClientCorsOrigin, ClientCorsOriginDto>
{
    public override partial ClientCorsOriginDto Map(ClientCorsOrigin source);
    public override partial void Map(ClientCorsOrigin source, ClientCorsOriginDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ClientRedirectUriToClientRedirectUriDtoMapper : MapperBase<ClientRedirectUri, ClientRedirectUriDto>
{
    public override partial ClientRedirectUriDto Map(ClientRedirectUri source);
    public override partial void Map(ClientRedirectUri source, ClientRedirectUriDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ClientPostLogoutRedirectUriToClientPostLogoutRedirectUriDtoMapper : MapperBase<ClientPostLogoutRedirectUri, ClientPostLogoutRedirectUriDto>
{
    public override partial ClientPostLogoutRedirectUriDto Map(ClientPostLogoutRedirectUri source);
    public override partial void Map(ClientPostLogoutRedirectUri source, ClientPostLogoutRedirectUriDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ClientIdPRestrictionToClientIdPRestrictionDtoMapper : MapperBase<ClientIdPRestriction, ClientIdPRestrictionDto>
{
    public override partial ClientIdPRestrictionDto Map(ClientIdPRestriction source);
    public override partial void Map(ClientIdPRestriction source, ClientIdPRestrictionDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ClientClaimToClientClaimDtoMapper : MapperBase<ClientClaim, ClientClaimDto>
{
    public override partial ClientClaimDto Map(ClientClaim source);
    public override partial void Map(ClientClaim source, ClientClaimDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ClientPropertyToClientPropertyDtoMapper : MapperBase<ClientProperty, ClientPropertyDto>
{
    public override partial ClientPropertyDto Map(ClientProperty source);
    public override partial void Map(ClientProperty source, ClientPropertyDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class ClientToClientDtoMapper : MapperBase<Client, ClientDto>
{
    public override partial ClientDto Map(Client source);
    public override partial void Map(Client source, ClientDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ApiScopeClaimToApiScopeClaimDtoMapper : MapperBase<ApiScopeClaim, ApiScopeClaimDto>
{
    public override partial ApiScopeClaimDto Map(ApiScopeClaim source);
    public override partial void Map(ApiScopeClaim source, ApiScopeClaimDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ApiScopePropertyToApiScopePropertyDtoMapper : MapperBase<ApiScopeProperty, ApiScopePropertyDto>
{
    public override partial ApiScopePropertyDto Map(ApiScopeProperty source);
    public override partial void Map(ApiScopeProperty source, ApiScopePropertyDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class ApiScopeToApiScopeDtoMapper : MapperBase<ApiScope, ApiScopeDto>
{
    public override partial ApiScopeDto Map(ApiScope source);
    public override partial void Map(ApiScope source, ApiScopeDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ApiResourcePropertyToApiResourcePropertyDtoMapper : MapperBase<ApiResourceProperty, ApiResourcePropertyDto>
{
    public override partial ApiResourcePropertyDto Map(ApiResourceProperty source);
    public override partial void Map(ApiResourceProperty source, ApiResourcePropertyDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ApiResourceSecretToApiResourceSecretDtoMapper : MapperBase<ApiResourceSecret, ApiResourceSecretDto>
{
    public override partial ApiResourceSecretDto Map(ApiResourceSecret source);
    public override partial void Map(ApiResourceSecret source, ApiResourceSecretDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ApiResourceScopeToApiResourceScopeDtoMapper : MapperBase<ApiResourceScope, ApiResourceScopeDto>
{
    public override partial ApiResourceScopeDto Map(ApiResourceScope source);
    public override partial void Map(ApiResourceScope source, ApiResourceScopeDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ApiResourceClaimToApiResourceClaimDtoMapper : MapperBase<ApiResourceClaim, ApiResourceClaimDto>
{
    public override partial ApiResourceClaimDto Map(ApiResourceClaim source);
    public override partial void Map(ApiResourceClaim source, ApiResourceClaimDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class ApiResourceToApiResourceDtoMapper : MapperBase<ApiResource, ApiResourceDto>
{
    public override partial ApiResourceDto Map(ApiResource source);
    public override partial void Map(ApiResource source, ApiResourceDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class IdentityResourceClaimToIdentityResourceClaimDtoMapper : MapperBase<IdentityResourceClaim, IdentityResourceClaimDto>
{
    public override partial IdentityResourceClaimDto Map(IdentityResourceClaim source);
    public override partial void Map(IdentityResourceClaim source, IdentityResourceClaimDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class IdentityResourcePropertyToIdentityResourcePropertyDtoMapper : MapperBase<IdentityResourceProperty, IdentityResourcePropertyDto>
{
    public override partial IdentityResourcePropertyDto Map(IdentityResourceProperty source);
    public override partial void Map(IdentityResourceProperty source, IdentityResourcePropertyDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class IdentityResourceToIdentityResourceDtoMapper : MapperBase<IdentityResource, IdentityResourceDto>
{
    public override partial IdentityResourceDto Map(IdentityResource source);
    public override partial void Map(IdentityResource source, IdentityResourceDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class PersistedGrantToPersistedGrantDtoMapper : MapperBase<PersistedGrant, PersistedGrantDto>
{
    public override partial PersistedGrantDto Map(PersistedGrant source);
    public override partial void Map(PersistedGrant source, PersistedGrantDto destination);
}
