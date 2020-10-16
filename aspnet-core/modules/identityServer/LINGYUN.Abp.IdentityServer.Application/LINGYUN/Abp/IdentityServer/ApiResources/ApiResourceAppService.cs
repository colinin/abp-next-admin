using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.IdentityServer.ApiResources;
using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;

namespace LINGYUN.Abp.IdentityServer.ApiResources
{
    [Authorize(AbpIdentityServerPermissions.ApiResources.Default)]
    public class ApiResourceAppService : AbpIdentityServerAppServiceBase, IApiResourceAppService
    {
        protected IApiResourceRepository ApiResourceRepository { get; }

        public ApiResourceAppService(
            IApiResourceRepository apiResourceRepository)
        {
            ApiResourceRepository = apiResourceRepository;
        }

        public virtual async Task<ApiResourceDto> GetAsync(Guid id)
        {
            var apiResource = await ApiResourceRepository.GetAsync(id);

            return ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource);
        }

        public virtual async Task<PagedResultDto<ApiResourceDto>> GetListAsync(ApiResourceGetByPagedInputDto input)
        {
            var apiResources = await ApiResourceRepository.GetListAsync(input.Sorting,
                input.SkipCount, input.MaxResultCount,
                input.Filter);
            // 未加Filter过滤器? 结果数不准
            var apiResourceCount = await ApiResourceRepository.GetCountAsync();

            return new PagedResultDto<ApiResourceDto>(apiResourceCount,
                ObjectMapper.Map<List<ApiResource>, List<ApiResourceDto>>(apiResources));
        }

        [Authorize(AbpIdentityServerPermissions.ApiResources.Create)]
        public virtual async Task<ApiResourceDto> CreateAsync(ApiResourceCreateDto input)
        {
            var apiResourceExists = await ApiResourceRepository.CheckNameExistAsync(input.Name);
            if (apiResourceExists)
            {
                throw new UserFriendlyException(L[AbpIdentityServerErrorConsts.ApiResourceNameExisted, input.Name]);
            }
            var apiResource = new ApiResource(GuidGenerator.Create(), input.Name,
                input.DisplayName, input.Description)
            {
                Enabled = input.Enabled
            };
            UpdateApiResourceByInput(apiResource, input);

            apiResource = await ApiResourceRepository.InsertAsync(apiResource);

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource);
        }

        [Authorize(AbpIdentityServerPermissions.ApiResources.Update)]
        public virtual async Task<ApiResourceDto> UpdateAsync(Guid id, ApiResourceUpdateDto input)
        {
            var apiResource = await ApiResourceRepository.GetAsync(id);
            apiResource.DisplayName = input.DisplayName ?? apiResource.DisplayName;
            apiResource.Description = input.Description ?? apiResource.Description;
            apiResource.Enabled = input.Enabled;

            UpdateApiResourceByInput(apiResource, input);

            apiResource = await ApiResourceRepository.UpdateAsync(apiResource);

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource);
        }

        [Authorize(AbpIdentityServerPermissions.ApiResources.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var apiResource = await ApiResourceRepository.GetAsync(id);
            await ApiResourceRepository.DeleteAsync(apiResource);

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        protected virtual void UpdateApiResourceByInput(ApiResource apiResource, ApiResourceCreateOrUpdateDto input)
        {
            // 删除不存在的UserClaim
            apiResource.UserClaims.RemoveAll(claim => !input.UserClaims.Contains(claim.Type));
            foreach (var inputClaim in input.UserClaims)
            {
                var userClaim = apiResource.FindClaim(inputClaim);
                if (userClaim == null)
                {
                    apiResource.AddUserClaim(inputClaim);
                }
            }

            // 删除不存在的Scope
            apiResource.Scopes.RemoveAll(scope => !input.Scopes.Any(inputScope => scope.Name == inputScope.Name));
            foreach (var inputScope in input.Scopes)
            {
                var scope = apiResource.FindScope(inputScope.Name);
                if (scope == null)
                {
                    scope = apiResource.AddScope(
                        inputScope.Name, inputScope.DisplayName, inputScope.Description,
                        inputScope.Required, inputScope.Emphasize, inputScope.ShowInDiscoveryDocument);
                }
                else
                {
                    scope.Required = inputScope.Required;
                    scope.Emphasize = inputScope.Emphasize;
                    scope.Description = inputScope.Description;
                    scope.DisplayName = inputScope.DisplayName;
                    scope.ShowInDiscoveryDocument = inputScope.ShowInDiscoveryDocument;
                    // 删除不存在的ScopeUserClaim
                    scope.UserClaims.RemoveAll(claim => !inputScope.UserClaims.Contains(claim.Type));
                }

                foreach (var inputScopeClaim in inputScope.UserClaims)
                {
                    var scopeUserClaim = scope.FindClaim(inputScopeClaim);
                    if (scopeUserClaim == null)
                    {
                        scope.AddUserClaim(inputScopeClaim);
                    }
                }
            }

            // 删除不存在的Secret
            apiResource.Secrets.RemoveAll(secret => !input.Secrets.Any(inputSecret => secret.Type == inputSecret.Type && secret.Value == inputSecret.Value));
            foreach (var inputSecret in input.Secrets)
            {
                // 第一次重复校验已经加密过的字符串
                if (apiResource.FindSecret(inputSecret.Value, inputSecret.Type) == null)
                {
                    var apiSecretValue = inputSecret.Value;
                    if (IdentityServerConstants.SecretTypes.SharedSecret.Equals(inputSecret.Type))
                    {
                        if (inputSecret.HashType == HashType.Sha256)
                        {
                            apiSecretValue = inputSecret.Value.Sha256();
                        }
                        else if (inputSecret.HashType == HashType.Sha512)
                        {
                            apiSecretValue = inputSecret.Value.Sha512();
                        }
                    }
                    // 加密之后还需要做一次校验 避免出现重复值
                    var secret = apiResource.FindSecret(apiSecretValue, inputSecret.Type);
                    if (secret == null)
                    {
                        apiResource.AddSecret(apiSecretValue, inputSecret.Expiration, inputSecret.Type, inputSecret.Description);
                    }
                }
            }
        }
    }
}
