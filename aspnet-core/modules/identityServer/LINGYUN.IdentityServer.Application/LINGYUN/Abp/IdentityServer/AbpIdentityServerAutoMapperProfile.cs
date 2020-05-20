using AutoMapper;
using LINGYUN.Abp.IdentityServer.ApiResources;
using LINGYUN.Abp.IdentityServer.Clients;
using LINGYUN.Abp.IdentityServer.IdentityResources;
using System.Collections.Generic;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.IdentityResources;

namespace LINGYUN.Abp.IdentityServer
{
    public class AbpIdentityServerAutoMapperProfile : Profile
    {
        public AbpIdentityServerAutoMapperProfile()
        {
            CreateMap<ClientSecret, ClientSecretDto>();
            CreateMap<ClientClaim, ClientClaimDto>();
            CreateMap<ClientCorsOrigin, ClientCorsOriginDto>();
            CreateMap<ClientGrantType, ClientGrantTypeDto>();
            CreateMap<ClientIdPRestriction, ClientIdPRestrictionDto>();
            CreateMap<ClientPostLogoutRedirectUri, ClientPostLogoutRedirectUriDto>();
            CreateMap<ClientProperty, ClientPropertyDto>();
            CreateMap<ClientRedirectUri, ClientRedirectUriDto>();
            CreateMap<ClientScope, ClientScopeDto>();
            CreateMap<Client, ClientDto>();

            CreateMap<ApiSecret, ApiSecretDto>();
            CreateMap<ApiScope, ApiScopeDto>();
            CreateMap<ApiScopeClaim, ApiScopeClaimDto>();
            CreateMap<ApiResourceClaim, ApiResourceClaimDto>();
            CreateMap<ApiResource, ApiResourceDto>();

            CreateMap<IdentityClaim, IdentityClaimDto>();
            CreateMap<IdentityResource, IdentityResourceDto>()
                .ForMember(dto => dto.Properties, map => map.MapFrom((src, d) =>
                {
                    var properties = new List<IdentityResourcePropertyDto>();
                    foreach (var property in src.Properties)
                    {
                        properties.Add(new IdentityResourcePropertyDto
                        {
                            Key = property.Key,
                            Value = property.Value
                        });
                    }
                    return properties;
                }));
        }
    }
}
