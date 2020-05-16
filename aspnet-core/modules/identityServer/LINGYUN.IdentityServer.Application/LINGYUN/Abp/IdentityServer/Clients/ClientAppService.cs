using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Security.Encryption;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public class ClientAppService : AbpIdentityServerAppServiceBase, IClientAppService
    {
        private IStringEncryptionService _encryptionService;
        protected IStringEncryptionService EncryptionService => LazyGetRequiredService(ref _encryptionService);

        protected IClientRepository ClientRepository { get; }

        public ClientAppService(IClientRepository clientRepository)
        {
            ClientRepository = clientRepository;
        }

        public virtual async Task<ClientClaimDto> AddClaimAsync(ClientClaimCreateDto clientClaimCreate)
        {
            var client = await ClientRepository.GetAsync(clientClaimCreate.ClientId);

            client.AddClaim(clientClaimCreate.Value, clientClaimCreate.Type);
            var clientClaim = client.FindClaim(clientClaimCreate.Value, clientClaimCreate.Type);

            return ObjectMapper.Map<ClientClaim, ClientClaimDto>(clientClaim);
        }

        public virtual async Task<ClientPropertyDto> AddPropertyAsync(ClientPropertyCreateDto clientPropertyCreate)
        {
            var client = await ClientRepository.GetAsync(clientPropertyCreate.ClientId);

            client.AddProperty(clientPropertyCreate.Key, clientPropertyCreate.Value);
            var clientProperty = client.FindProperty(clientPropertyCreate.Key, clientPropertyCreate.Value);

            return ObjectMapper.Map<ClientProperty, ClientPropertyDto>(clientProperty);
        }

        public virtual async Task<ClientSecretDto> AddSecretAsync(ClientSecretCreateDto clientSecretCreate)
        {
            var client = await ClientRepository.GetAsync(clientSecretCreate.ClientId);

            var clientSecretValue = EncryptionService.Encrypt(clientSecretCreate.Value);

            client.AddSecret(clientSecretValue, clientSecretCreate.Expiration, 
                clientSecretCreate.Type, clientSecretCreate.Description);
            
            var clientSecret = client.FindSecret(clientSecretValue, clientSecretCreate.Type);

            return ObjectMapper.Map<ClientSecret, ClientSecretDto>(clientSecret);
        }

        public virtual async Task<ClientDto> CreateAsync(ClientCreateDto clientCreate)
        {
            var clientIdExists = await ClientRepository.CheckClientIdExistAsync(clientCreate.ClientId);
            if(clientIdExists)
            {
                throw new UserFriendlyException(L[AbpIdentityServerErrorConsts.ClientIdExisted, clientCreate.ClientId]);
            }
            var client = new Client(GuidGenerator.Create(), clientCreate.ClientId);
            client.ClientName = clientCreate.ClientName;
            client.Description = clientCreate.Description;
            foreach(var grantType in clientCreate.AllowedGrantTypes)
            {
                client.AddGrantType(grantType.GrantType);
            }

            client = await ClientRepository.InsertAsync(client, true);

            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        public virtual async Task DeleteAsync(ClientGetByIdInputDto clientGetByIdInput)
        {
            await ClientRepository.DeleteAsync(clientGetByIdInput.Id);
        }

        public virtual async Task DeleteClaimAsync(ClientClaimGetByKeyInputDto clientClaimGetByKey)
        {
            var client = await ClientRepository.GetAsync(clientClaimGetByKey.ClientId);
            client.Claims.RemoveAll(claim => claim.Type.Equals(clientClaimGetByKey.Type));
        }

        public virtual async Task DeletePropertyAsync(ClientPropertyGetByKeyDto clientPropertyGetByKey)
        {
            var client = await ClientRepository.GetAsync(clientPropertyGetByKey.ClientId);
            client.Properties.RemoveAll(property => property.Key.Equals(clientPropertyGetByKey.Key));
        }

        public virtual async Task DeleteSecretAsync(ClientSecretGetByTypeDto clientSecretGetByType)
        {
            var client = await ClientRepository.GetAsync(clientSecretGetByType.ClientId);
            client.ClientSecrets.RemoveAll(secret => secret.Type.Equals(clientSecretGetByType.Type));
        }

        public virtual async Task<ClientDto> GetAsync(ClientGetByIdInputDto clientGetById)
        {
            var client = await ClientRepository.GetAsync(clientGetById.Id);

            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        public virtual async Task<PagedResultDto<ClientDto>> GetAsync(ClientGetByPagedInputDto clientGetByPaged)
        {
            // Abp官方IdentityServer项目不支持Filter过滤...
            var clients = await ClientRepository.GetListAsync(clientGetByPaged.Sorting,
                clientGetByPaged.SkipCount, clientGetByPaged.MaxResultCount, true);

            var clientCount = await ClientRepository.GetCountAsync();

            return new PagedResultDto<ClientDto>(clientCount, 
                ObjectMapper.Map<List<Client>, List<ClientDto>>(clients));
        }

        public virtual async Task<ClientDto> UpdateAsync(ClientUpdateDto clientUpdate)
        {
            var client = await ClientRepository.GetAsync(clientUpdate.Id);

            #region Basic Property
            client.ConcurrencyStamp = clientUpdate.ConcurrencyStamp;
            client.ClientId = clientUpdate.ClientId ?? client.ClientId;
            client.ClientUri = clientUpdate.ClientUri ?? client.ClientUri;
            client.ClientName = clientUpdate.ClientName ?? client.ClientName;
            client.AbsoluteRefreshTokenLifetime = clientUpdate.AbsoluteRefreshTokenLifetime
                ?? client.AbsoluteRefreshTokenLifetime;
            client.AccessTokenLifetime = clientUpdate.AccessTokenLifetime
                ?? client.AccessTokenLifetime;
            client.AccessTokenType = clientUpdate.AccessTokenType ?? client.AccessTokenType;
            client.AllowAccessTokensViaBrowser = clientUpdate.AllowAccessTokensViaBrowser
                ?? client.AllowAccessTokensViaBrowser;
            client.AllowOfflineAccess = clientUpdate.AllowOfflineAccess
                ?? client.AllowOfflineAccess;
            client.AllowPlainTextPkce = clientUpdate.AllowPlainTextPkce ?? client.AllowPlainTextPkce;
            client.AllowRememberConsent = clientUpdate.AllowRememberConsent ?? client.AllowRememberConsent;
            client.AlwaysIncludeUserClaimsInIdToken = clientUpdate.AlwaysIncludeUserClaimsInIdToken
                ?? client.AlwaysIncludeUserClaimsInIdToken;
            client.AlwaysSendClientClaims = clientUpdate.AlwaysSendClientClaims ?? client.AlwaysSendClientClaims;
            client.AuthorizationCodeLifetime = clientUpdate.AuthorizationCodeLifetime
                ?? client.AuthorizationCodeLifetime;
            client.BackChannelLogoutSessionRequired = clientUpdate.BackChannelLogoutSessionRequired
                ?? client.BackChannelLogoutSessionRequired;

            client.BackChannelLogoutUri = clientUpdate.BackChannelLogoutUri
                ?? client.BackChannelLogoutUri;
            client.ClientClaimsPrefix = clientUpdate.ClientClaimsPrefix ?? client.ClientClaimsPrefix;
            client.ConsentLifetime = clientUpdate.ConsentLifetime ?? client.ConsentLifetime;
            client.Description = clientUpdate.Description ?? client.Description;
            client.DeviceCodeLifetime = clientUpdate.DeviceCodeLifetime ?? client.DeviceCodeLifetime;
            client.Enabled = clientUpdate.Enabled ?? client.Enabled;
            client.EnableLocalLogin = clientUpdate.EnableLocalLogin ?? client.EnableLocalLogin;
            client.FrontChannelLogoutSessionRequired = clientUpdate.FrontChannelLogoutSessionRequired
                ?? client.FrontChannelLogoutSessionRequired;
            client.FrontChannelLogoutUri = clientUpdate.FrontChannelLogoutUri ?? client.FrontChannelLogoutUri;

            client.IdentityTokenLifetime = clientUpdate.IdentityTokenLifetime ?? client.IdentityTokenLifetime;
            client.IncludeJwtId = clientUpdate.IncludeJwtId ?? client.IncludeJwtId;
            client.LogoUri = clientUpdate.LogoUri ?? client.LogoUri;
            client.PairWiseSubjectSalt = clientUpdate.PairWiseSubjectSalt ?? client.PairWiseSubjectSalt;
            client.ProtocolType = clientUpdate.ProtocolType ?? client.ProtocolType;
            client.RefreshTokenExpiration = clientUpdate.RefreshTokenExpiration ?? client.RefreshTokenExpiration;
            client.RefreshTokenUsage = clientUpdate.RefreshTokenUsage ?? client.RefreshTokenUsage;
            client.RequireClientSecret = clientUpdate.RequireClientSecret ?? client.RequireClientSecret;
            client.RequireConsent = clientUpdate.RequireConsent ?? client.RequireConsent;

            client.RequirePkce = clientUpdate.RequirePkce ?? client.RequirePkce;
            client.SlidingRefreshTokenLifetime = clientUpdate.SlidingRefreshTokenLifetime
                ?? client.SlidingRefreshTokenLifetime;
            client.UpdateAccessTokenClaimsOnRefresh = clientUpdate.UpdateAccessTokenClaimsOnRefresh
                ?? client.UpdateAccessTokenClaimsOnRefresh;

            client.UserCodeType = clientUpdate.UserCodeType ?? client.UserCodeType;
            client.UserSsoLifetime = clientUpdate.UserSsoLifetime ?? client.UserSsoLifetime;
            #endregion

            #region AllowScope

            foreach(var scope in clientUpdate.AllowedScopes)
            {
                var clientScope = client.FindScope(scope.Scope);
                if (clientScope == null)
                {
                    client.AddScope(scope.Scope);
                }
            }

            #endregion

            #region RedirectUris

            foreach(var redirect in clientUpdate.RedirectUris)
            {
                var clientRedirect = client.FindRedirectUri(redirect.RedirectUri);
                if(clientRedirect == null)
                {
                    client.AddRedirectUri(redirect.RedirectUri);
                }
            }

            #endregion

            #region AllowedGrantTypes

            foreach (var grantType in clientUpdate.AllowedGrantTypes)
            {
                var clientGrantType = client.FindGrantType(grantType.GrantType);
                if (clientGrantType == null)
                {
                    client.AddGrantType(grantType.GrantType);
                }
            }

            #endregion

            #region AllowedCorsOrigins

            foreach (var corgOrigin in clientUpdate.AllowedCorsOrigins)
            {
                var clientCorsOrigin = client.FindCorsOrigin(corgOrigin.Origin);
                if (clientCorsOrigin == null)
                {
                    client.AddCorsOrigin(corgOrigin.Origin);
                }
            }

            #endregion

            #region PostLogoutRedirectUris

            foreach (var logoutRedirect in clientUpdate.PostLogoutRedirectUris)
            {
                var clientLogoutRedirect = client.FindPostLogoutRedirectUri(logoutRedirect.PostLogoutRedirectUri);
                if (clientLogoutRedirect == null)
                {
                    client.AddPostLogoutRedirectUri(logoutRedirect.PostLogoutRedirectUri);
                }
            }

            #endregion

            #region IdentityProviderRestrictions

            foreach (var provider in clientUpdate.IdentityProviderRestrictions)
            {
                var clientIdentityProvider = client.FindIdentityProviderRestriction(provider.Provider);
                if (clientIdentityProvider == null)
                {
                    client.AddIdentityProviderRestriction(provider.Provider);
                }
            }

            #endregion

            client = await ClientRepository.UpdateAsync(client, true);

            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        public virtual async Task<ClientClaimDto> UpdateClaimAsync(ClientClaimUpdateDto clientClaimUpdate)
        {
            var client = await ClientRepository.GetAsync(clientClaimUpdate.ClientId);

            var clientClaim = client.Claims.FirstOrDefault(claim => claim.Type.Equals(clientClaimUpdate.Type));
            if(clientClaim == null)
            {
                throw new UserFriendlyException(L[AbpIdentityServerErrorConsts.ClientClaimNotFound, clientClaimUpdate.Type]);
            }
            clientClaim.Value = clientClaimUpdate.Value;

            return ObjectMapper.Map<ClientClaim, ClientClaimDto>(clientClaim);
        }

        public virtual async Task<ClientPropertyDto> UpdatePropertyAsync(ClientPropertyUpdateDto clientPropertyUpdate)
        {
            var client = await ClientRepository.GetAsync(clientPropertyUpdate.ClientId);

            var clientProperty = client.Properties
                .FirstOrDefault(property => property.Key.Equals(clientPropertyUpdate.Key));
            if (clientProperty == null)
            {
                throw new UserFriendlyException(
                    L[AbpIdentityServerErrorConsts.ClientPropertyNotFound, clientPropertyUpdate.Key]);
            }
            clientProperty.Value = clientPropertyUpdate.Value;

            return ObjectMapper.Map<ClientProperty, ClientPropertyDto>(clientProperty);
        }

        public virtual async Task<ClientSecretDto> UpdateSecretAsync(ClientSecretUpdateDto clientSecretUpdate)
        {
            var client = await ClientRepository.GetAsync(clientSecretUpdate.ClientId);

            var clientSecret = client.ClientSecrets
                .FirstOrDefault(secret => secret.Type.Equals(clientSecretUpdate.Type));
            if (clientSecret == null)
            {
                throw new UserFriendlyException(
                    L[AbpIdentityServerErrorConsts.ClientSecretNotFound, clientSecretUpdate.Type]);
            }
            clientSecret.Value = EncryptionService.Encrypt(clientSecretUpdate.Value);

            return ObjectMapper.Map<ClientSecret, ClientSecretDto>(clientSecret);
        }
    }
}
