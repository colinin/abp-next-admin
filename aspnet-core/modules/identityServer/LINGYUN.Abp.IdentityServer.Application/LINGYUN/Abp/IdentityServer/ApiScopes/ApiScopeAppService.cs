using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.IdentityServer.ApiScopes;

namespace LINGYUN.Abp.IdentityServer.ApiScopes
{
    [Authorize(AbpIdentityServerPermissions.ApiScopes.Default)]
    public class ApiScopeAppService : AbpIdentityServerAppServiceBase, IApiScopeAppService
    {
        protected IApiScopeRepository ApiScopeRepository { get; }

        public ApiScopeAppService(
            IApiScopeRepository apiScopeRepository)
        {
            ApiScopeRepository = apiScopeRepository;
        }

        [Authorize(AbpIdentityServerPermissions.ApiScopes.Create)]
        public virtual async Task<ApiScopeDto> CreateAsync(ApiScopeCreateDto input)
        {
            if (await ApiScopeRepository.CheckNameExistAsync(input.Name))
            {
                throw new UserFriendlyException(L[AbpIdentityServerErrorConsts.ApiScopeNameExisted, input.Name]);
            }
            var apiScope = new ApiScope(
                GuidGenerator.Create(), 
                input.Name, 
                input.DisplayName,
                input.Description,
                input.Enabled,
                input.Required, 
                input.Emphasize,
                input.ShowInDiscoveryDocument);

            await UpdateApiScopeByInputAsync(apiScope, input);

            await CurrentUnitOfWork.SaveChangesAsync();

            apiScope = await ApiScopeRepository.InsertAsync(apiScope);

            return ObjectMapper.Map<ApiScope, ApiScopeDto>(apiScope);
        }

        [Authorize(AbpIdentityServerPermissions.ApiScopes.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var apiScope = await ApiScopeRepository.GetAsync(id);

            await ApiScopeRepository.DeleteAsync(apiScope);

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public virtual async Task<ApiScopeDto> GetAsync(Guid id)
        {
            var apiScope = await ApiScopeRepository.GetAsync(id);

            return ObjectMapper.Map<ApiScope, ApiScopeDto>(apiScope);
        }

        public virtual async Task<PagedResultDto<ApiScopeDto>> GetListAsync(GetApiScopeInput input)
        {
            var totalCount = await ApiScopeRepository
                .GetCountAsync(input.Filter);

            var apiScopes = await ApiScopeRepository
                .GetListAsync(
                    input.Sorting,
                    input.SkipCount, input.MaxResultCount,
                    input.Filter);

            return new PagedResultDto<ApiScopeDto>(totalCount,
                ObjectMapper.Map<List<ApiScope>, List<ApiScopeDto>>(apiScopes));
        }

        [Authorize(AbpIdentityServerPermissions.ApiScopes.Update)]
        public virtual async Task<ApiScopeDto> UpdateAsync(Guid id, ApiScopeUpdateDto input)
        {
            var apiScope = await ApiScopeRepository.GetAsync(id);

            await UpdateApiScopeByInputAsync(apiScope, input);
            apiScope = await ApiScopeRepository.UpdateAsync(apiScope);

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<ApiScope, ApiScopeDto>(apiScope);
        }

        protected virtual async Task UpdateApiScopeByInputAsync(ApiScope apiScope, ApiScopeCreateOrUpdateDto input)
        {
            if (!string.Equals(apiScope.Description, input.Description, StringComparison.InvariantCultureIgnoreCase))
            {
                apiScope.Description = input.Description;
            }
            if (!string.Equals(apiScope.DisplayName, input.DisplayName, StringComparison.InvariantCultureIgnoreCase))
            {
                apiScope.DisplayName = input.DisplayName;
            }
            apiScope.Emphasize = input.Emphasize;
            apiScope.Enabled = input.Enabled;
            apiScope.Required = input.Required;
            apiScope.ShowInDiscoveryDocument = input.ShowInDiscoveryDocument;

            if (await IsGrantAsync(AbpIdentityServerPermissions.ApiScopes.ManageClaims))
            {
                // 删除不存在的UserClaim
                apiScope.UserClaims.RemoveAll(claim => !input.UserClaims.Any(inputClaim => claim.Type == inputClaim.Type));
                foreach (var inputClaim in input.UserClaims)
                {
                    var userClaim = apiScope.FindClaim(inputClaim.Type);
                    if (userClaim == null)
                    {
                        apiScope.AddUserClaim(inputClaim.Type);
                    }
                }
            }

            if (await IsGrantAsync(AbpIdentityServerPermissions.ApiScopes.ManageProperties))
            {
                // 删除不存在的Property
                apiScope.Properties.RemoveAll(prop => !input.Properties.Any(inputProp => prop.Key == inputProp.Key));
                foreach (var inputProp in input.Properties)
                {
                    var identityResourceProperty = apiScope.FindProperty(inputProp.Key);
                    if (identityResourceProperty == null)
                    {
                        apiScope.AddProperty(inputProp.Key, inputProp.Value);
                    }
                    else
                    {
                        identityResourceProperty.Value = inputProp.Value;
                    }
                }
            }
        }
    }
}
