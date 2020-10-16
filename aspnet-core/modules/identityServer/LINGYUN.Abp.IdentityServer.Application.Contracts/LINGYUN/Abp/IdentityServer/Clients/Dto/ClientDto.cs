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

        public List<string> AllowedScopes { get; set; }

        public List<ClientSecretDto> ClientSecrets { get; set; }

        public List<string> AllowedGrantTypes { get; set; }

        public List<string> AllowedCorsOrigins { get; set; }

        public List<string> RedirectUris { get; set; }

        public List<string> PostLogoutRedirectUris { get; set; }

        public List<string> IdentityProviderRestrictions { get; set; }

        public List<ClientClaimDto> Claims { get; set; }

        public Dictionary<string, string> Properties { get; set; }
        public ClientDto()
        {
            Claims = new List<ClientClaimDto>();
            Properties = new Dictionary<string, string>();
            AllowedScopes = new List<string>();
            ClientSecrets = new List<ClientSecretDto>();
            RedirectUris = new List<string>();
            AllowedGrantTypes = new List<string>();
            AllowedCorsOrigins = new List<string>();
            PostLogoutRedirectUris = new List<string>();
            IdentityProviderRestrictions = new List<string>();
        }
    }
}
