using AutoMapper;
using LINGYUN.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.Clients;

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
        }
    }
}
