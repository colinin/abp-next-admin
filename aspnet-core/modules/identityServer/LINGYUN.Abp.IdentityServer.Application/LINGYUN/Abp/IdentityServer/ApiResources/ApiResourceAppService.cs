using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
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

        public virtual async Task<ApiResourceDto> GetAsync(ApiResourceGetByIdInputDto apiResourceGetById)
        {
            var apiResource = await ApiResourceRepository.GetAsync(apiResourceGetById.Id);

            return ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource);
        }

        public virtual async Task<PagedResultDto<ApiResourceDto>> GetAsync(ApiResourceGetByPagedInputDto apiResourceGetByPaged)
        {
            var apiResources = await ApiResourceRepository.GetListAsync(apiResourceGetByPaged.Sorting,
                apiResourceGetByPaged.SkipCount, apiResourceGetByPaged.MaxResultCount);
            var apiResourceCount = await ApiResourceRepository.GetCountAsync();

            return new PagedResultDto<ApiResourceDto>(apiResourceCount,
                ObjectMapper.Map<List<ApiResource>, List<ApiResourceDto>>(apiResources));
        }

        [Authorize(AbpIdentityServerPermissions.ApiResources.Create)]
        public virtual async Task<ApiResourceDto> CreateAsync(ApiResourceCreateDto apiResourceCreate)
        {
            var apiResourceExists = await ApiResourceRepository.CheckNameExistAsync(apiResourceCreate.Name);
            if (apiResourceExists)
            {
                throw new UserFriendlyException(L[AbpIdentityServerErrorConsts.ApiResourceNameExisted, apiResourceCreate.Name]);
            }
            var apiResource = new ApiResource(GuidGenerator.Create(), apiResourceCreate.Name, 
                apiResourceCreate.DisplayName, apiResourceCreate.Description);
            apiResource.Enabled = apiResourceCreate.Enabled;
            foreach(var userClaim in apiResourceCreate.UserClaims)
            {
                apiResource.AddUserClaim(userClaim.Type);
            }
            apiResource = await ApiResourceRepository.InsertAsync(apiResource);
            return ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource);
        }

        [Authorize(AbpIdentityServerPermissions.ApiResources.Update)]
        public virtual async Task<ApiResourceDto> UpdateAsync(ApiResourceUpdateDto apiResourceUpdate)
        {
            var apiResource = await ApiResourceRepository.GetAsync(apiResourceUpdate.Id);
            apiResource.DisplayName = apiResourceUpdate.DisplayName ?? apiResource.DisplayName;
            apiResource.Description = apiResourceUpdate.Description ?? apiResource.Description;
            apiResource.Enabled = apiResourceUpdate.Enabled;

            apiResource.RemoveAllUserClaims();
            foreach (var userClaim in apiResourceUpdate.UserClaims)
            {
                apiResource.AddUserClaim(userClaim.Type);
            }
            apiResource = await ApiResourceRepository.UpdateAsync(apiResource);
            return ObjectMapper.Map<ApiResource, ApiResourceDto>(apiResource);
        }

        [Authorize(AbpIdentityServerPermissions.ApiResources.Delete)]
        public virtual async Task DeleteAsync(ApiResourceGetByIdInputDto apiResourceGetById)
        {
            var apiResource = await ApiResourceRepository.GetAsync(apiResourceGetById.Id);
            await ApiResourceRepository.DeleteAsync(apiResource);
        }

        [Authorize(AbpIdentityServerPermissions.ApiResources.Secrets.Create)]
        public virtual async Task<ApiSecretDto> AddSecretAsync(ApiSecretCreateDto apiSecretCreate)
        {
            var apiResource = await ApiResourceRepository.GetAsync(apiSecretCreate.ApiResourceId);
            var apiSecretValue = apiSecretCreate.Value;
            var apiResourceSecret = apiResource.FindSecret(apiSecretValue, apiSecretCreate.Type);
            if(apiResourceSecret == null)
            {
                if (IdentityServerConstants.SecretTypes.SharedSecret.Equals(apiSecretCreate.Type))
                {
                    if (apiSecretCreate.HashType == HashType.Sha256)
                    {
                        apiSecretValue = apiSecretCreate.Value.Sha256();
                    }
                    else if (apiSecretCreate.HashType == HashType.Sha512)
                    {
                        apiSecretValue = apiSecretCreate.Value.Sha512();
                    }
                }
                apiResource.AddSecret(apiSecretValue, apiSecretCreate.Expiration, apiSecretCreate.Type, apiSecretCreate.Description);
                apiResourceSecret = apiResource.FindSecret(apiSecretValue, apiSecretCreate.Type);
            }

            return ObjectMapper.Map<ApiSecret, ApiSecretDto>(apiResourceSecret);
        }

        [Authorize(AbpIdentityServerPermissions.ApiResources.Secrets.Delete)]
        public virtual async Task DeleteSecretAsync(ApiSecretGetByTypeInputDto apiSecretGetByType)
        {
            var apiResource = await ApiResourceRepository.GetAsync(apiSecretGetByType.ApiResourceId);
            apiResource.RemoveSecret(apiSecretGetByType.Value, apiSecretGetByType.Type);
        }

        [Authorize(AbpIdentityServerPermissions.ApiResources.Scope.Create)]
        public virtual async Task<ApiScopeDto> AddScopeAsync(ApiScopeCreateDto apiScopeCreate)
        {
            var apiResource = await ApiResourceRepository.GetAsync(apiScopeCreate.ApiResourceId);
            var apiResourceScope = apiResource.FindScope(apiScopeCreate.Name);
            if (apiResourceScope == null)
            {
                apiResource.AddScope(apiScopeCreate.Name, apiScopeCreate.DisplayName, apiScopeCreate.Description,
                    apiScopeCreate.Required, apiScopeCreate.Emphasize, apiScopeCreate.ShowInDiscoveryDocument);
                apiResourceScope = apiResource.FindScope(apiScopeCreate.Name);
            }
            return ObjectMapper.Map<ApiScope, ApiScopeDto>(apiResourceScope);
        }

        [Authorize(AbpIdentityServerPermissions.ApiResources.Scope.Delete)]
        public virtual async Task DeleteScopeAsync(ApiScopeGetByNameInputDto apiScopeGetByName)
        {
            var apiResource = await ApiResourceRepository.GetAsync(apiScopeGetByName.ApiResourceId);
            apiResource.RemoveScope(apiScopeGetByName.Name);
        }
    }
}
