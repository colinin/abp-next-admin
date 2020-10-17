using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.Clients;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public class ClientUpdateDto : ClientCreateOrUpdateDto
    {

        [StringLength(ClientConsts.ClientUriMaxLength)]
        public string ClientUri { get; set; }

        [StringLength(ClientConsts.LogoUriMaxLength)]
        public string LogoUri { get; set; }

        public bool Enabled { get; set; }

        [StringLength(ClientConsts.ProtocolTypeMaxLength)]
        public string ProtocolType { get; set; }

        public bool RequireClientSecret { get; set; }

        public bool RequireConsent { get; set; }

        public bool AllowRememberConsent { get; set; }

        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }

        public bool RequirePkce { get; set; }

        public bool AllowPlainTextPkce { get; set; }

        public bool AllowAccessTokensViaBrowser { get; set; }

        [StringLength(ClientConsts.FrontChannelLogoutUriMaxLength)]
        public string FrontChannelLogoutUri { get; set; }

        public bool FrontChannelLogoutSessionRequired { get; set; }

        [StringLength(ClientConsts.BackChannelLogoutUriMaxLength)]
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

        [StringLength(ClientConsts.ClientClaimsPrefixMaxLength)]
        public string ClientClaimsPrefix { get; set; }

        [StringLength(ClientConsts.PairWiseSubjectSaltMaxLength)]
        public string PairWiseSubjectSalt { get; set; }

        public int? UserSsoLifetime { get; set; }

        [StringLength(ClientConsts.UserCodeTypeMaxLength)]
        public string UserCodeType { get; set; }

        public int DeviceCodeLifetime { get; set; }
        /// <summary>
        /// Api资源(AllowScopes)
        /// </summary>
        public List<string> ApiResources { get; set; }
        /// <summary>
        /// 身份资源(AllowScopes)
        /// </summary>
        public List<string> IdentityResources { get; set; }
        /// <summary>
        /// 允许同源
        /// </summary>
        public List<string> AllowedCorsOrigins { get; set; }
        /// <summary>
        /// 重定向uri
        /// </summary>
        public List<string> RedirectUris { get; set; }
        /// <summary>
        /// 登出重定向uri
        /// </summary>
        public List<string> PostLogoutRedirectUris { get; set; }
        /// <summary>
        /// 限制提供商
        /// </summary>
        public List<string> IdentityProviderRestrictions { get; set; }
        /// <summary>
        /// 属性
        /// </summary>
        public Dictionary<string, string> Properties { get; set; }
        /// <summary>
        /// 密钥
        /// </summary>
        public List<SecretCreateOrUpdateDto> Secrets { get; set; }
        /// <summary>
        /// 声明
        /// </summary>
        public List<ClientClaimDto> Claims { get; set; }

        public ClientUpdateDto()
        {
            Enabled = true;
            DeviceCodeLifetime = 300;
            ApiResources = new List<string>();
            IdentityResources = new List<string>();
            RedirectUris = new List<string>();
            AllowedCorsOrigins = new List<string>();
            PostLogoutRedirectUris = new List<string>();
            IdentityProviderRestrictions = new List<string>();
            Properties = new Dictionary<string, string>();
            Secrets = new List<SecretCreateOrUpdateDto>();
            Claims = new List<ClientClaimDto>();
        }
    }
}
