using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.IdentityServer.Grants;

namespace LINGYUN.Abp.IdentityServer.Grants
{
    [Authorize(AbpIdentityServerPermissions.Grants.Default)]
    public class PersistedGrantAppService : AbpIdentityServerAppServiceBase, IPersistedGrantAppService
    {
        protected IPersistentGrantRepository PersistentGrantRepository { get; }

        public PersistedGrantAppService(
            IPersistentGrantRepository persistentGrantRepository)
        {
            PersistentGrantRepository = persistentGrantRepository;
        }

        [Authorize(AbpIdentityServerPermissions.Grants.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var persistedGrant = await PersistentGrantRepository.GetAsync(id);

            await PersistentGrantRepository.DeleteAsync(persistedGrant);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public virtual async Task<PersistedGrantDto> GetAsync(Guid id)
        {
            var persistedGrant = await PersistentGrantRepository.GetAsync(id);

            return ObjectMapper.Map<PersistedGrant, PersistedGrantDto>(persistedGrant);
        }

        public virtual async Task<PagedResultDto<PersistedGrantDto>> GetListAsync(GetPersistedGrantInput input)
        {
            var persistenGrantCount = await PersistentGrantRepository
                .GetCountAsync(
                    input.SubjectId, input.Filter);

            var persistenGrants = await PersistentGrantRepository
                .GetListAsync(
                    input.SubjectId, input.Filter, input.Sorting,
                    input.SkipCount, input.MaxResultCount);

            return new PagedResultDto<PersistedGrantDto>(persistenGrantCount,
                ObjectMapper.Map<List<PersistedGrant>, List<PersistedGrantDto>>(persistenGrants));
        }
    }
}
