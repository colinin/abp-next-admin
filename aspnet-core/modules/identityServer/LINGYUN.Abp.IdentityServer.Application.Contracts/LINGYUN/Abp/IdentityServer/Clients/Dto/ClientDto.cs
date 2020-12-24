using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public class ClientDto : FullAuditedEntityDto<Guid>
    {
        public string ClientId { get; set; }

        public string ClientName { get; set; }

        public string Description { get; set; }

        public string ClientUri { get; set; }

        public string LogoUri { get; set; }

        public bool Enabled { get; set; }

        public string ProtocolType { get; set; }

        public bool RequireClientSecret { get; set; }

        public bool RequireConsent { get; set; }

        public bool AllowRememberConsent { get; set; }

        public bool RequireRequestObject { get; set; }

        public string AllowedIdentityTokenSigningAlgorithms { get; set; }

        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }

        public bool RequirePkce { get; set; }

        public bool AllowPlainTextPkce { get; set; }

        public bool AllowAccessTokensViaBrowser { get; set; }

        public string FrontChannelLogoutUri { get; set; }

        public bool FrontChannelLogoutSessionRequired { get; set; }

        public string BackChannelLogoutUri { get; set; }

        public bool BackChannelLogoutSessionRequired { get; set; }

        public bool AllowOfflineAccess { get; set; }

        public int IdentityTokenLifetime { get; set; }

        public int AccessTokenLifetime { get; set; }

        public int AuthorizationCodeLifetime { get; set; }

        public int? ConsentLifetime { get; set; }

        public int AbsoluteRefreshTokenLifetime { get; set; }

        public int SlidingRefreshTokenLifetime { get; set; }

        public int RefreshTokenUsage { get; set; }

        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }

        public int RefreshTokenExpiration { get; set; }

        public int AccessTokenType { get; set; }

        public bool EnableLocalLogin { get; set; }

        public bool IncludeJwtId { get; set; }

        public bool AlwaysSendClientClaims { get; set; }

        public string ClientClaimsPrefix { get; set; }

        public string PairWiseSubjectSalt { get; set; }

        public int? UserSsoLifetime { get; set; }

        public string UserCodeType { get; set; }

        public int DeviceCodeLifetime { get; set; }

        public string ConcurrencyStamp { get; set; }

        public List<ClientScopeDto> AllowedScopes { get; set; }

        public List<ClientSecretDto> ClientSecrets { get; set; }

        public List<ClientGrantTypeDto> AllowedGrantTypes { get; set; }

        public List<ClientCorsOriginDto> AllowedCorsOrigins { get; set; }

        public List<ClientRedirectUriDto> RedirectUris { get; set; }

        public List<ClientPostLogoutRedirectUriDto> PostLogoutRedirectUris { get; set; }

        public List<ClientIdPRestrictionDto> IdentityProviderRestrictions { get; set; }

        public List<ClientClaimDto> Claims { get; set; }

        public List<ClientPropertyDto> Properties { get; set; }
        public ClientDto()
        {
            Claims = new List<ClientClaimDto>();
            Properties = new List<ClientPropertyDto>();
            AllowedScopes = new List<ClientScopeDto>();
            ClientSecrets = new List<ClientSecretDto>();
            RedirectUris = new List<ClientRedirectUriDto>();
            AllowedGrantTypes = new List<ClientGrantTypeDto>();
            AllowedCorsOrigins = new List<ClientCorsOriginDto>();
            PostLogoutRedirectUris = new List<ClientPostLogoutRedirectUriDto>();
            IdentityProviderRestrictions = new List<ClientIdPRestrictionDto>();
        }
    }
}
