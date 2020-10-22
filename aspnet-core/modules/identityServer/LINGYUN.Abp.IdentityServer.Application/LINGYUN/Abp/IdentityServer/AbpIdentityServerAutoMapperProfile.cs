using AutoMapper;
using LINGYUN.Abp.IdentityServer.ApiResources;
using LINGYUN.Abp.IdentityServer.Clients;
using LINGYUN.Abp.IdentityServer.IdentityResources;
using System.Collections.Generic;
using System.Linq;
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
            CreateMap<List<ClientProperty>, Dictionary<string, string>>()
                .ConstructUsing((props, ctx) =>
                {
                    var properties = new Dictionary<string, string>();
                    foreach (var prop in props)
                    {
                        properties.Add(prop.Key, prop.Value);
                    }
                    return properties;
                });
            CreateMap<Client, ClientDto>()
                .ForMember(dto => dto.AllowedCorsOrigins, map => map.MapFrom(client => client.AllowedCorsOrigins.Select(origin => origin.Origin).ToList()))
                .ForMember(dto => dto.AllowedGrantTypes, map => map.MapFrom(client => client.AllowedGrantTypes.Select(grantType => grantType.GrantType).ToList()))
                .ForMember(dto => dto.AllowedScopes, map => map.MapFrom(client => client.AllowedScopes.Select(scope => scope.Scope).ToList()))
                .ForMember(dto => dto.IdentityProviderRestrictions, map => map.MapFrom(client => client.IdentityProviderRestrictions.Select(provider => provider.Provider).ToList()))
                .ForMember(dto => dto.PostLogoutRedirectUris, map => map.MapFrom(client => client.PostLogoutRedirectUris.Select(uri => uri.PostLogoutRedirectUri).ToList()))
                .ForMember(dto => dto.RedirectUris, map => map.MapFrom(client => client.RedirectUris.Select(uri => uri.RedirectUri).ToList()));

            CreateMap<ApiSecret, ApiSecretDto>();
            CreateMap<ApiScope, ApiScopeDto>();
            CreateMap<ApiResource, ApiResourceDto>()
                .ForMember(dto => dto.UserClaims, map => map.MapFrom(src => src.UserClaims.Select(claim => claim.Type).ToList()))
                .MapExtraProperties();

            CreateMap<IdentityResource, IdentityResourceDto>()
                .ForMember(dto => dto.UserClaims, map => map.MapFrom(src => src.UserClaims.Select(claim => claim.Type).ToList()))
                .MapExtraProperties();
        }
    }
}
