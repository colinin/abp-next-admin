using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.IdentityServer.IdentityResources;

namespace LINGYUN.Abp.IdentityServer.IdentityResources
{
    [Authorize(AbpIdentityServerPermissions.IdentityResources.Default)]
    public class IdentityResourceAppService : AbpIdentityServerAppServiceBase, IIdentityResourceAppService
    {
        protected IIdentityResourceRepository IdentityResourceRepository { get; }

        public IdentityResourceAppService(
            IIdentityResourceRepository identityResourceRepository)
        {
            IdentityResourceRepository = identityResourceRepository;
        }

        public virtual async Task<IdentityResourceDto> GetAsync(Guid id)
        {
            var identityResource = await IdentityResourceRepository.GetAsync(id);

            return ObjectMapper.Map<IdentityResource, IdentityResourceDto>(identityResource);
        }

        public virtual async Task<PagedResultDto<IdentityResourceDto>> GetListAsync(IdentityResourceGetByPagedDto input)
        {
            var identityResources = await IdentityResourceRepository.GetListAsync(input.Sorting,
                input.SkipCount, input.MaxResultCount,
                input.Filter);
            var identityResourceCount = await IdentityResourceRepository.GetCountAsync();

            return new PagedResultDto<IdentityResourceDto>(identityResourceCount,
                ObjectMapper.Map<List<IdentityResource>, List<IdentityResourceDto>>(identityResources));
        }

        [Authorize(AbpIdentityServerPermissions.IdentityResources.Create)]
        public virtual async Task<IdentityResourceDto> CreateAsync(IdentityResourceCreateOrUpdateDto input)
        {
            var identityResourceExists = await IdentityResourceRepository.CheckNameExistAsync(input.Name);
            if (identityResourceExists)
            {
                throw new UserFriendlyException(L[AbpIdentityServerErrorConsts.IdentityResourceNameExisted, input.Name]);
            }
            var identityResource = new IdentityResource(GuidGenerator.Create(), input.Name, input.DisplayName,
                input.Description, input.Enabled, input.Required, input.Emphasize,
                input.ShowInDiscoveryDocument);
            await UpdateApiResourceByInputAsync(identityResource, input);

            await CurrentUnitOfWork.SaveChangesAsync();

            identityResource = await IdentityResourceRepository.InsertAsync(identityResource);

            return ObjectMapper.Map<IdentityResource, IdentityResourceDto>(identityResource);
        }

        [Authorize(AbpIdentityServerPermissions.IdentityResources.Update)]
        public virtual async Task<IdentityResourceDto> UpdateAsync(Guid id, IdentityResourceCreateOrUpdateDto input)
        {
            var identityResource = await IdentityResourceRepository.GetAsync(id);
            await UpdateApiResourceByInputAsync(identityResource, input);
            identityResource = await IdentityResourceRepository.UpdateAsync(identityResource);

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityResource, IdentityResourceDto>(identityResource);
        }

        [Authorize(AbpIdentityServerPermissions.IdentityResources.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await IdentityResourceRepository.DeleteAsync(id);
        }

        protected virtual async Task UpdateApiResourceByInputAsync(IdentityResource identityResource, IdentityResourceCreateOrUpdateDto input)
        {
            if (!string.Equals(identityResource.Name, input.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                identityResource.Name = input.Name;
            }
            if (!string.Equals(identityResource.Description, input.Description, StringComparison.InvariantCultureIgnoreCase))
            {
                identityResource.Description = input.Description;
            }
            if (!string.Equals(identityResource.DisplayName, input.DisplayName, StringComparison.InvariantCultureIgnoreCase))
            {
                identityResource.DisplayName = input.DisplayName;
            }
            identityResource.Emphasize = input.Emphasize;
            identityResource.Enabled = input.Enabled;
            identityResource.Required = input.Required;
            identityResource.ShowInDiscoveryDocument = input.ShowInDiscoveryDocument;

            if (await IsGrantAsync(AbpIdentityServerPermissions.IdentityResources.ManageClaims))
            {
                // 删除不存在的UserClaim
                identityResource.UserClaims.RemoveAll(claim => input.UserClaims.Any(inputClaim => claim.Type == inputClaim.Type));
                foreach (var inputClaim in input.UserClaims)
                {
                    var userClaim = identityResource.FindUserClaim(inputClaim.Type);
                    if (userClaim == null)
                    {
                        identityResource.AddUserClaim(inputClaim.Type);
                    }
                }
            }

            if (await IsGrantAsync(AbpIdentityServerPermissions.IdentityResources.ManageProperties))
            {
                // 删除不存在的Property
                identityResource.Properties.RemoveAll(prop => !input.Properties.Any(inputProp => prop.Key == inputProp.Key));
                foreach (var inputProp in input.Properties)
                {
                    var identityResourceProperty = identityResource.FindProperty(inputProp.Key);
                    if (identityResourceProperty == null)
                    {
                        identityResource.AddProperty(inputProp.Key, inputProp.Value);
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
