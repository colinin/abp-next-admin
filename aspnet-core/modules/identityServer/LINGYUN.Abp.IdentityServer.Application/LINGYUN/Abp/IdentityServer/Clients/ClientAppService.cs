using LINGYUN.Abp.IdentityServer.ApiResources;
using LINGYUN.Abp.IdentityServer.IdentityResources;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.IdentityServer.Clients;
using Client = Volo.Abp.IdentityServer.Clients.Client;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    [Authorize(AbpIdentityServerPermissions.Clients.Default)]
    public class ClientAppService : AbpIdentityServerAppServiceBase, IClientAppService
    {
        protected IClientRepository ClientRepository { get; }
        protected IApiResourceRepository ApiResourceRepository { get; }
        protected IIdentityResourceRepository IdentityResourceRepository { get; }

        public ClientAppService(
            IClientRepository clientRepository,
            IApiResourceRepository apiResourceRepository,
            IIdentityResourceRepository identityResourceRepository)
        {
            ClientRepository = clientRepository;
            ApiResourceRepository = apiResourceRepository;
            IdentityResourceRepository = identityResourceRepository;
        }

        [Authorize(AbpIdentityServerPermissions.Clients.Create)]
        public virtual async Task<ClientDto> CreateAsync(ClientCreateDto clientCreate)
        {
            var clientIdExists = await ClientRepository.CheckClientIdExistAsync(clientCreate.ClientId);
            if(clientIdExists)
            {
                throw new UserFriendlyException(L[AbpIdentityServerErrorConsts.ClientIdExisted, clientCreate.ClientId]);
            }
            var client = new Client(GuidGenerator.Create(), clientCreate.ClientId)
            {
                ClientName = clientCreate.ClientName,
                Description = clientCreate.Description
            };
            foreach (var inputGrantType in clientCreate.AllowedGrantTypes)
            {
                client.AddGrantType(inputGrantType.GrantType);
            }

            client = await ClientRepository.InsertAsync(client);

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        [Authorize(AbpIdentityServerPermissions.Clients.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var client = await ClientRepository.GetAsync(id);
            await ClientRepository.DeleteAsync(client);

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public virtual async Task<ClientDto> GetAsync(Guid id)
        {
            var client = await ClientRepository.GetAsync(id);

            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        public virtual async Task<PagedResultDto<ClientDto>> GetListAsync(ClientGetByPagedDto input)
        {
            var clients = await ClientRepository.GetListAsync(input.Sorting,
                input.SkipCount, input.MaxResultCount,
                input.Filter);

            var clientCount = await ClientRepository.GetCountAsync();

            return new PagedResultDto<ClientDto>(clientCount, 
                ObjectMapper.Map<List<Client>, List<ClientDto>>(clients));
        }

        [Authorize(AbpIdentityServerPermissions.Clients.Update)]
        public virtual async Task<ClientDto> UpdateAsync(Guid id, ClientUpdateDto input)
        {
            var client = await ClientRepository.GetAsync(id);

            #region Basic
            if (!string.Equals(client.ClientId, input.ClientId, StringComparison.InvariantCultureIgnoreCase))
            {
                client.ClientId = input.ClientId;
            }
            if (!string.Equals(client.ClientUri, input.ClientUri, StringComparison.InvariantCultureIgnoreCase))
            {
                client.ClientUri = input.ClientUri;
            }
            if (!string.Equals(client.ClientName, input.ClientName, StringComparison.InvariantCultureIgnoreCase))
            {
                client.ClientName = input.ClientName;
            }
            if (!string.Equals(client.BackChannelLogoutUri, input.BackChannelLogoutUri, StringComparison.InvariantCultureIgnoreCase))
            {
                client.BackChannelLogoutUri = input.BackChannelLogoutUri;
            }
            if (!string.Equals(client.FrontChannelLogoutUri, input.FrontChannelLogoutUri, StringComparison.InvariantCultureIgnoreCase))
            {
                client.FrontChannelLogoutUri = input.FrontChannelLogoutUri;
            }
            if (!string.Equals(client.ClientClaimsPrefix, input.ClientClaimsPrefix, StringComparison.InvariantCultureIgnoreCase))
            {
                client.ClientClaimsPrefix = input.ClientClaimsPrefix;
            }
            if (!string.Equals(client.Description, input.Description, StringComparison.InvariantCultureIgnoreCase))
            {
                client.Description = input.Description;
            }
            if (!string.Equals(client.LogoUri, input.LogoUri, StringComparison.InvariantCultureIgnoreCase))
            {
                client.LogoUri = input.LogoUri;
            }
            if (!string.Equals(client.UserCodeType, input.UserCodeType, StringComparison.InvariantCultureIgnoreCase))
            {
                client.UserCodeType = input.UserCodeType;
            }
            if (!string.Equals(client.PairWiseSubjectSalt, input.PairWiseSubjectSalt, StringComparison.InvariantCultureIgnoreCase))
            {
                client.PairWiseSubjectSalt = input.PairWiseSubjectSalt;
            }
            if (!string.Equals(client.ProtocolType, input.ProtocolType, StringComparison.InvariantCultureIgnoreCase))
            {
                client.ProtocolType = input.ProtocolType;
            }
            if (!string.Equals(client.AllowedIdentityTokenSigningAlgorithms, input.AllowedIdentityTokenSigningAlgorithms, StringComparison.InvariantCultureIgnoreCase))
            {
                client.AllowedIdentityTokenSigningAlgorithms = input.AllowedIdentityTokenSigningAlgorithms;
            }

            client.AbsoluteRefreshTokenLifetime = input.AbsoluteRefreshTokenLifetime;
            client.AccessTokenLifetime = input.AccessTokenLifetime;
            client.AccessTokenType = input.AccessTokenType;
            client.AllowAccessTokensViaBrowser = input.AllowAccessTokensViaBrowser;
            client.AllowOfflineAccess = input.AllowOfflineAccess;
            client.AllowPlainTextPkce = input.AllowPlainTextPkce;
            client.AllowRememberConsent = input.AllowRememberConsent;
            client.AlwaysIncludeUserClaimsInIdToken = input.AlwaysIncludeUserClaimsInIdToken;
            client.AlwaysSendClientClaims = input.AlwaysSendClientClaims;
            client.AuthorizationCodeLifetime = input.AuthorizationCodeLifetime;
            client.BackChannelLogoutSessionRequired = input.BackChannelLogoutSessionRequired;
            client.DeviceCodeLifetime = input.DeviceCodeLifetime;
            client.ConsentLifetime = input.ConsentLifetime ?? client.ConsentLifetime;
            client.Enabled = input.Enabled;
            client.RequireRequestObject = input.RequireRequestObject;
            client.EnableLocalLogin = input.EnableLocalLogin;
            client.FrontChannelLogoutSessionRequired = input.FrontChannelLogoutSessionRequired;
            client.IdentityTokenLifetime = input.IdentityTokenLifetime;
            client.IncludeJwtId = input.IncludeJwtId;
            client.RefreshTokenExpiration = input.RefreshTokenExpiration;
            client.RefreshTokenUsage = input.RefreshTokenUsage;
            client.RequireClientSecret = input.RequireClientSecret;
            client.RequireConsent = input.RequireConsent;
            client.RequirePkce = input.RequirePkce;
            client.SlidingRefreshTokenLifetime = input.SlidingRefreshTokenLifetime;
            client.UpdateAccessTokenClaimsOnRefresh = input.UpdateAccessTokenClaimsOnRefresh;
            client.UserSsoLifetime = input.UserSsoLifetime ?? client.UserSsoLifetime;
            #endregion

            #region AllowScope
            // 删除未在身份资源和Api资源中的作用域
            client.AllowedScopes.RemoveAll(scope => !input.AllowedScopes.Any(inputScope => scope.Scope == inputScope.Scope));
            foreach (var inputScope in input.AllowedScopes)
            {
                if (client.FindScope(inputScope.Scope) == null)
                {
                    client.AddScope(inputScope.Scope);
                }
            }

            #endregion

            #region RedirectUris
            // 删除不存在的uri
            client.RedirectUris.RemoveAll(uri => !input.RedirectUris.Any(inputRedirectUri => uri.RedirectUri == inputRedirectUri.RedirectUri));
            foreach (var inputRedirectUri in input.RedirectUris)
            {
                if (client.FindRedirectUri(inputRedirectUri.RedirectUri) == null)
                {
                    client.AddRedirectUri(inputRedirectUri.RedirectUri);
                }
            }

            #endregion

            #region AllowedGrantTypes
            // 删除不存在的验证类型
            client.AllowedGrantTypes.RemoveAll(grantType => !input.AllowedGrantTypes.Any(inputGrantType => grantType.GrantType == inputGrantType.GrantType));
            foreach (var inputGrantType in input.AllowedGrantTypes)
            {
                if (client.FindGrantType(inputGrantType.GrantType) == null)
                {
                    client.AddGrantType(inputGrantType.GrantType);
                }
            }

            #endregion

            #region AllowedCorsOrigins
            // 删除不存在的同源域名
            client.AllowedCorsOrigins.RemoveAll(corsOrigin => !input.AllowedCorsOrigins.Any(inputCorsOrigin => corsOrigin.Origin == inputCorsOrigin.Origin));
            foreach (var inputCorsOrigin in input.AllowedCorsOrigins)
            {
                if (client.FindCorsOrigin(inputCorsOrigin.Origin) == null)
                {
                    client.AddCorsOrigin(inputCorsOrigin.Origin);
                }
            }

            #endregion

            #region PostLogoutRedirectUris

            // 删除不存在的登录重定向域名
            client.PostLogoutRedirectUris.RemoveAll(uri => 
                !input.PostLogoutRedirectUris.Any(inputLogoutRedirectUri => uri.PostLogoutRedirectUri == inputLogoutRedirectUri.PostLogoutRedirectUri));
            foreach (var inputLogoutRedirectUri in input.PostLogoutRedirectUris)
            {
                if (client.FindPostLogoutRedirectUri(inputLogoutRedirectUri.PostLogoutRedirectUri) == null)
                {
                    client.AddPostLogoutRedirectUri(inputLogoutRedirectUri.PostLogoutRedirectUri);
                }
            }

            #endregion

            #region IdentityProviderRestrictions

            // 删除身份认证限制提供商
            client.IdentityProviderRestrictions.RemoveAll(provider => 
                !input.IdentityProviderRestrictions.Any(inputProvider => provider.Provider == inputProvider.Provider));
            foreach (var inputProvider in input.IdentityProviderRestrictions)
            {
                if (client.FindIdentityProviderRestriction(inputProvider.Provider) == null)
                {
                    client.AddIdentityProviderRestriction(inputProvider.Provider);
                }
            }

            #endregion

            #region Secrets

            if (await IsGrantAsync(AbpIdentityServerPermissions.Clients.ManageSecrets))
            {
                // 移除已经不存在的客户端密钥
                client.ClientSecrets.RemoveAll(secret => !input.ClientSecrets.Any(inputSecret => secret.Value == inputSecret.Value && secret.Type == inputSecret.Type));
                foreach (var inputSecret in input.ClientSecrets)
                {
                    // 先对加密过的进行过滤
                    if (client.FindSecret(inputSecret.Value, inputSecret.Type) != null)
                    {
                        continue;
                    }
                    var inputSecretValue = inputSecret.Value.Sha256(); // TODO: 通过可选配置来加密

                    var clientSecret = client.FindSecret(inputSecretValue, inputSecret.Type);
                    if (clientSecret == null)
                    {
                        client.AddSecret(inputSecretValue, inputSecret.Expiration, inputSecret.Type, inputSecret.Description);
                    }
                }
            }

            #endregion

            #region Properties

            if (await IsGrantAsync(AbpIdentityServerPermissions.Clients.ManageProperties))
            {
                // 移除不存在的属性
                client.Properties.RemoveAll(prop => !input.Properties.Any(inputProp => prop.Key == inputProp.Key));
                foreach (var inputProp in input.Properties)
                {
                    var findProp = client.FindProperty(inputProp.Key);
                    if (findProp == null)
                    {
                        client.AddProperty(inputProp.Key, inputProp.Value);
                    }
                    else
                    {
                        findProp.Value = inputProp.Value;
                    }
                }
            }


            #endregion

            #region Claims

            if (await IsGrantAsync(AbpIdentityServerPermissions.Clients.ManageClaims))
            {
                // 移除已经不存在的客户端声明
                client.Claims.RemoveAll(secret => !input.Claims.Any(inputClaim => secret.Value == inputClaim.Value && secret.Type == inputClaim.Type));
                foreach (var inputClaim in input.Claims)
                {
                    if (client.FindClaim(inputClaim.Value, inputClaim.Type) == null)
                    {
                        client.AddClaim(inputClaim.Value, inputClaim.Type);
                    }
                }
            }
            
            #endregion

            client = await ClientRepository.UpdateAsync(client);

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<Client, ClientDto>(client);
        }

        /// <summary>
        /// 克隆客户端
        /// </summary>
        /// <remarks>
        /// 实现参考 Skoruba.IdentityServer4.Admin 项目
        /// https://github.com/skoruba/IdentityServer4.Admin.git
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(AbpIdentityServerPermissions.Clients.Clone)]
        public virtual async Task<ClientDto> CloneAsync(Guid id, ClientCloneDto input)
        {
            var clientIdExists = await ClientRepository.CheckClientIdExistAsync(input.ClientId);
            if (clientIdExists)
            {
                throw new UserFriendlyException(L[AbpIdentityServerErrorConsts.ClientIdExisted, input.ClientId]);
            }
            var srcClient = await ClientRepository.GetAsync(id);

            var client = new Client(GuidGenerator.Create(), input.ClientId)
            {
                ClientName = input.ClientName,
                Description = input.Description,
                AbsoluteRefreshTokenLifetime = srcClient.AbsoluteRefreshTokenLifetime,
                AccessTokenLifetime = srcClient.AccessTokenLifetime,
                AccessTokenType = srcClient.AccessTokenType,
                AllowAccessTokensViaBrowser = srcClient.AllowAccessTokensViaBrowser,
                AllowOfflineAccess = srcClient.AllowOfflineAccess,
                AllowPlainTextPkce = srcClient.AllowPlainTextPkce,
                AllowRememberConsent = srcClient.AllowRememberConsent,
                AlwaysIncludeUserClaimsInIdToken = srcClient.AlwaysIncludeUserClaimsInIdToken,
                AlwaysSendClientClaims = srcClient.AlwaysSendClientClaims,
                AuthorizationCodeLifetime = srcClient.AuthorizationCodeLifetime,
                BackChannelLogoutSessionRequired = srcClient.BackChannelLogoutSessionRequired,

                BackChannelLogoutUri = srcClient.BackChannelLogoutUri,
                ClientClaimsPrefix = srcClient.ClientClaimsPrefix,
                ConsentLifetime = srcClient.ConsentLifetime,
                DeviceCodeLifetime = srcClient.DeviceCodeLifetime,
                Enabled = srcClient.Enabled,
                EnableLocalLogin = srcClient.EnableLocalLogin,
                FrontChannelLogoutSessionRequired = srcClient.FrontChannelLogoutSessionRequired,
                FrontChannelLogoutUri = srcClient.FrontChannelLogoutUri,

                IdentityTokenLifetime = srcClient.IdentityTokenLifetime,
                IncludeJwtId = srcClient.IncludeJwtId,
                LogoUri = srcClient.LogoUri,
                PairWiseSubjectSalt = srcClient.PairWiseSubjectSalt,
                ProtocolType = srcClient.ProtocolType,
                RefreshTokenExpiration = srcClient.RefreshTokenExpiration,
                RefreshTokenUsage = srcClient.RefreshTokenUsage,
                RequireClientSecret = srcClient.RequireClientSecret,
                RequireConsent = srcClient.RequireConsent,

                RequirePkce = srcClient.RequirePkce,
                SlidingRefreshTokenLifetime = srcClient.SlidingRefreshTokenLifetime,
                UpdateAccessTokenClaimsOnRefresh = srcClient.UpdateAccessTokenClaimsOnRefresh,

                UserCodeType = srcClient.UserCodeType,
                UserSsoLifetime = srcClient.UserSsoLifetime
            };

            if (input.CopyAllowedCorsOrigin)
            {
                foreach(var corsOrigin in srcClient.AllowedCorsOrigins)
                {
                    client.AddCorsOrigin(corsOrigin.Origin);
                }
            }
            if (input.CopyAllowedGrantType)
            {
                foreach (var grantType in srcClient.AllowedGrantTypes)
                {
                    client.AddGrantType(grantType.GrantType);
                }
            }
            if (input.CopyAllowedScope)
            {
                foreach (var scope in srcClient.AllowedScopes)
                {
                    client.AddScope(scope.Scope);
                }
            }
            if (input.CopyClaim)
            {
                foreach (var claim in srcClient.Claims)
                {
                    client.AddClaim(claim.Value, claim.Type);
                }
            }
            if (input.CopySecret)
            {
                foreach (var secret in srcClient.ClientSecrets)
                {
                    client.AddSecret(secret.Value, secret.Expiration, secret.Type, secret.Description);
                }
            }
            if (input.CopyIdentityProviderRestriction)
            {
                foreach (var provider in srcClient.IdentityProviderRestrictions)
                {
                    client.AddIdentityProviderRestriction(provider.Provider);
                }
            }
            if (input.CopyPostLogoutRedirectUri)
            {
                foreach (var uri in srcClient.PostLogoutRedirectUris)
                {
                    client.AddPostLogoutRedirectUri(uri.PostLogoutRedirectUri);
                }
            }
            if (input.CopyPropertie)
            {
                foreach (var property in srcClient.Properties)
                {
                    client.AddProperty(property.Key, property.Value);
                }
            }
            if (input.CopyRedirectUri)
            {
                foreach (var uri in srcClient.RedirectUris)
                {
                    client.AddRedirectUri(uri.RedirectUri);
                }
            }
            client = await ClientRepository.InsertAsync(client);

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<Client, ClientDto>(client);
        }
        /// <summary>
        /// 查询可用的Api资源
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ListResultDto<string>> GetAssignableApiResourcesAsync()
        {
            var resourceNames = await ApiResourceRepository.GetNamesAsync();

            return new ListResultDto<string>(resourceNames);
        }
        /// <summary>
        /// 查询可用的身份资源
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ListResultDto<string>> GetAssignableIdentityResourcesAsync()
        {
            var resourceNames = await IdentityResourceRepository.GetNamesAsync();

            return new ListResultDto<string>(resourceNames);
        }
        /// <summary>
        /// 查询所有不重复的跨域地址
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ListResultDto<string>> GetAllDistinctAllowedCorsOriginsAsync()
        {
            var corsOrigins = await ClientRepository.GetAllDistinctAllowedCorsOriginsAsync();

            return new ListResultDto<string>(corsOrigins);
        }
    }
}
