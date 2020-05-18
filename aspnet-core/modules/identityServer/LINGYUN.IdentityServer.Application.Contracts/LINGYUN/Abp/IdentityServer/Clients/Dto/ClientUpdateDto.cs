using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.Clients;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public class ClientUpdateDto 
    {
        [Required]
        [StringLength(1000)]
        public string ConcurrencyStamp { get; set; }

        [StringLength(ClientConsts.ClientIdMaxLength)]
        public string ClientId { get; set; }

        [StringLength(ClientConsts.ClientNameMaxLength)]
        public string ClientName { get; set; }

        [StringLength(ClientConsts.DescriptionMaxLength)]
        public string Description { get; set; }

        [StringLength(ClientConsts.ClientUriMaxLength)]
        public string ClientUri { get; set; }

        [StringLength(ClientConsts.LogoUriMaxLength)]
        public string LogoUri { get; set; }

        public bool? Enabled { get; set; }

        [StringLength(ClientConsts.ProtocolTypeMaxLength)]
        public string ProtocolType { get; set; }

        public bool? RequireClientSecret { get; set; }

        public bool? RequireConsent { get; set; }

        public bool? AllowRememberConsent { get; set; }

        public bool? AlwaysIncludeUserClaimsInIdToken { get; set; }

        public bool? RequirePkce { get; set; }

        public bool? AllowPlainTextPkce { get; set; }

        public bool? AllowAccessTokensViaBrowser { get; set; }

        [StringLength(ClientConsts.FrontChannelLogoutUriMaxLength)]
        public string FrontChannelLogoutUri { get; set; }

        public bool? FrontChannelLogoutSessionRequired { get; set; }

        [StringLength(ClientConsts.BackChannelLogoutUriMaxLength)]
        public string BackChannelLogoutUri { get; set; }

        public bool? BackChannelLogoutSessionRequired { get; set; }

        public bool? AllowOfflineAccess { get; set; }

        public int? IdentityTokenLifetime { get; set; }

        public int? AccessTokenLifetime { get; set; }

        public int? AuthorizationCodeLifetime { get; set; }

        public int? ConsentLifetime { get; set; }

        public int? AbsoluteRefreshTokenLifetime { get; set; }

        public int? SlidingRefreshTokenLifetime { get; set; }

        public int? RefreshTokenUsage { get; set; }

        public bool? UpdateAccessTokenClaimsOnRefresh { get; set; }

        public int? RefreshTokenExpiration { get; set; }

        public int? AccessTokenType { get; set; }

        public bool? EnableLocalLogin { get; set; }

        public bool? IncludeJwtId { get; set; }

        public bool? AlwaysSendClientClaims { get; set; }

        [StringLength(ClientConsts.ClientClaimsPrefixMaxLength)]
        public string ClientClaimsPrefix { get; set; }

        [StringLength(ClientConsts.PairWiseSubjectSaltMaxLength)]
        public string PairWiseSubjectSalt { get; set; }

        public int? UserSsoLifetime { get; set; }

        [StringLength(ClientConsts.UserCodeTypeMaxLength)]
        public string UserCodeType { get; set; }

        public int? DeviceCodeLifetime { get; set; }

        public List<ClientScopeDto> AllowedScopes { get; set; }

        public List<ClientGrantTypeDto> AllowedGrantTypes { get; set; }

        public List<ClientCorsOriginDto> AllowedCorsOrigins { get; set; }

        public List<ClientRedirectUriDto> RedirectUris { get; set; }

        public List<ClientPostLogoutRedirectUriDto> PostLogoutRedirectUris { get; set; }

        public List<ClientIdPRestrictionDto> IdentityProviderRestrictions { get; set; }
        public ClientUpdateDto()
        {
            Enabled = true;
            DeviceCodeLifetime = 300;
            AllowedScopes = new List<ClientScopeDto>();
            RedirectUris = new List<ClientRedirectUriDto>();
            AllowedGrantTypes = new List<ClientGrantTypeDto>();
            AllowedCorsOrigins = new List<ClientCorsOriginDto>();
            PostLogoutRedirectUris = new List<ClientPostLogoutRedirectUriDto>();
            IdentityProviderRestrictions = new List<ClientIdPRestrictionDto>();
        }
    }
}
