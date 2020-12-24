using System.Collections.Generic;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public class ClientUpdateDto : ClientCreateOrUpdateDto
    {

        [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.ClientUriMaxLength))]
        public string ClientUri { get; set; }

        [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.LogoUriMaxLength))]
        public string LogoUri { get; set; }

        public bool Enabled { get; set; }

        [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.ProtocolTypeMaxLength))]
        public string ProtocolType { get; set; }

        public bool RequireClientSecret { get; set; }

        [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.AllowedIdentityTokenSigningAlgorithms))]
        public string AllowedIdentityTokenSigningAlgorithms { get; set; }

        public bool RequireConsent { get; set; } = false;

        public bool RequireRequestObject { get; set; }

        public bool AllowRememberConsent { get; set; }

        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }

        public bool RequirePkce { get; set; } = true;

        public bool AllowPlainTextPkce { get; set; }

        public bool AllowAccessTokensViaBrowser { get; set; }

        [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.FrontChannelLogoutUriMaxLength))]
        public string FrontChannelLogoutUri { get; set; }

        public bool FrontChannelLogoutSessionRequired { get; set; }

        [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.BackChannelLogoutUriMaxLength))]
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

        [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.ClientClaimsPrefixMaxLength))]
        public string ClientClaimsPrefix { get; set; }

        [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.PairWiseSubjectSaltMaxLength))]
        public string PairWiseSubjectSalt { get; set; }

        public int? UserSsoLifetime { get; set; }

        [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.UserCodeTypeMaxLength))]
        public string UserCodeType { get; set; }

        public int DeviceCodeLifetime { get; set; }

        public List<ClientScopeDto> AllowedScopes { get; set; }

        public List<ClientSecretDto> ClientSecrets { get; set; }

        public List<ClientCorsOriginDto> AllowedCorsOrigins { get; set; }

        public List<ClientRedirectUriDto> RedirectUris { get; set; }

        public List<ClientPostLogoutRedirectUriDto> PostLogoutRedirectUris { get; set; }

        public List<ClientIdPRestrictionDto> IdentityProviderRestrictions { get; set; }

        public List<ClientPropertyDto> Properties { get; set; }
        /// <summary>
        /// 声明
        /// </summary>
        public List<ClientClaimDto> Claims { get; set; }

        public ClientUpdateDto()
        {
            Enabled = true;
            DeviceCodeLifetime = 300;
            AllowedScopes = new List<ClientScopeDto>();
            RedirectUris = new List<ClientRedirectUriDto>();
            AllowedCorsOrigins = new List<ClientCorsOriginDto>();
            PostLogoutRedirectUris = new List<ClientPostLogoutRedirectUriDto>();
            IdentityProviderRestrictions = new List<ClientIdPRestrictionDto>();
            Properties = new List<ClientPropertyDto>();
            ClientSecrets = new List<ClientSecretDto>();
            Claims = new List<ClientClaimDto>();
        }
    }
}
