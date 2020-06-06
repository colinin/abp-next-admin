using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
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

        public virtual async Task<IdentityResourceDto> GetAsync(IdentityResourceGetByIdInputDto identityResourceGetById)
        {
            var identityResource = await IdentityResourceRepository.GetAsync(identityResourceGetById.Id);

            return ObjectMapper.Map<IdentityResource, IdentityResourceDto>(identityResource);
        }

        public virtual async Task<PagedResultDto<IdentityResourceDto>> GetAsync(IdentityResourceGetByPagedInputDto identityResourceGetByPaged)
        {
            var identityResources = await IdentityResourceRepository.GetListAsync(identityResourceGetByPaged.Sorting,
                identityResourceGetByPaged.SkipCount, identityResourceGetByPaged.MaxResultCount,
                identityResourceGetByPaged.Filter, true);
            var identityResourceCount = await IdentityResourceRepository.GetCountAsync();

            return new PagedResultDto<IdentityResourceDto>(identityResourceCount,
                ObjectMapper.Map<List<IdentityResource>, List<IdentityResourceDto>>(identityResources));
        }

        [Authorize(AbpIdentityServerPermissions.IdentityResources.Create)]
        public virtual async Task<IdentityResourceDto> CreateAsync(IdentityResourceCreateDto identityResourceCreate)
        {
            var identityResourceExists = await IdentityResourceRepository.CheckNameExistAsync(identityResourceCreate.Name);
            if (identityResourceExists)
            {
                throw new UserFriendlyException(L[AbpIdentityServerErrorConsts.IdentityResourceNameExisted, identityResourceCreate.Name]);
            }
            var identityResource = new IdentityResource(GuidGenerator.Create(), identityResourceCreate.Name, identityResourceCreate.DisplayName,
                identityResourceCreate.Description, identityResourceCreate.Enabled, identityResourceCreate.Required, identityResourceCreate.Emphasize,
                identityResourceCreate.ShowInDiscoveryDocument);
            foreach(var claim in identityResourceCreate.UserClaims)
            {
                identityResource.AddUserClaim(claim.Type);
            }
            identityResource = await IdentityResourceRepository.InsertAsync(identityResource);

            return ObjectMapper.Map<IdentityResource, IdentityResourceDto>(identityResource);
        }

        [Authorize(AbpIdentityServerPermissions.IdentityResources.Update)]
        public virtual async Task<IdentityResourceDto> UpdateAsync(IdentityResourceUpdateDto identityResourceUpdate)
        {
            var identityResource = await IdentityResourceRepository.GetAsync(identityResourceUpdate.Id);
            identityResource.ConcurrencyStamp = identityResourceUpdate.ConcurrencyStamp;
            identityResource.Name = identityResourceUpdate.Name ?? identityResource.Name;
            identityResource.DisplayName = identityResourceUpdate.DisplayName ?? identityResource.DisplayName;
            identityResource.Description = identityResourceUpdate.Description ?? identityResource.Description;
            identityResource.Enabled = identityResourceUpdate.Enabled;
            identityResource.Emphasize = identityResourceUpdate.Emphasize;
            identityResource.ShowInDiscoveryDocument = identityResourceUpdate.ShowInDiscoveryDocument;
            if (identityResourceUpdate.UserClaims.Count > 0)
            {
                identityResource.RemoveAllUserClaims();
                foreach (var claim in identityResourceUpdate.UserClaims)
                {
                    identityResource.AddUserClaim(claim.Type);
                }
            }
            identityResource = await IdentityResourceRepository.UpdateAsync(identityResource);

            return ObjectMapper.Map<IdentityResource, IdentityResourceDto>(identityResource);
        }

        [Authorize(AbpIdentityServerPermissions.IdentityResources.Delete)]
        public virtual async Task DeleteAsync(IdentityResourceGetByIdInputDto identityResourceGetById)
        {
            await IdentityResourceRepository.DeleteAsync(identityResourceGetById.Id);
        }

        [Authorize(AbpIdentityServerPermissions.IdentityResources.Properties.Create)]
        public virtual async Task<IdentityResourcePropertyDto> AddPropertyAsync(IdentityResourcePropertyCreateDto identityResourcePropertyCreate)
        {
            var identityResource = await IdentityResourceRepository.GetAsync(identityResourcePropertyCreate.IdentityResourceId);

            if (identityResource.Properties.ContainsKey(identityResourcePropertyCreate.Key))
            {
                throw new UserFriendlyException(L[AbpIdentityServerErrorConsts.IdentityResourcePropertyExisted, identityResourcePropertyCreate.Key]);
            }
            identityResource.ConcurrencyStamp = identityResourcePropertyCreate.ConcurrencyStamp;
            identityResource.Properties.Add(identityResourcePropertyCreate.Key, identityResourcePropertyCreate.Value);

            await IdentityResourceRepository.UpdateAsync(identityResource);
            return new IdentityResourcePropertyDto
            {
                Key = identityResourcePropertyCreate.Key,
                Value = identityResourcePropertyCreate.Value
            };
        }

        [Authorize(AbpIdentityServerPermissions.IdentityResources.Properties.Delete)]
        public virtual async Task DeletePropertyAsync(IdentityResourcePropertyGetByKeyDto identityResourcePropertyGetByKey)
        {
            var identityResource = await IdentityResourceRepository.GetAsync(identityResourcePropertyGetByKey.IdentityResourceId);

            if (!identityResource.Properties.ContainsKey(identityResourcePropertyGetByKey.Key))
            {
                throw new UserFriendlyException(L[AbpIdentityServerErrorConsts.IdentityResourcePropertyNotFound, identityResourcePropertyGetByKey.Key]);
            }
            identityResource.Properties.Remove(identityResourcePropertyGetByKey.Key);
            await IdentityResourceRepository.UpdateAsync(identityResource);
        }
    }
}
