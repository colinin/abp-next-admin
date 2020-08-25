using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.IdentityServer.Grants;

namespace LINGYUN.Abp.IdentityServer.Grants
{
    public class PersistedGrantAppService : AbpIdentityServerAppServiceBase
    {
        protected IPersistentGrantRepository PersistentGrantRepository { get; }
        public virtual async Task<PagedResultDto<PersistedGrantDto>> GetListAsync(PersistedGrantGetPagedDto input)
        {
            var persistenGrantCount = await PersistentGrantRepository.GetCountAsync(
                input.SubjectId, input.Filter);

            var persistenGrants = await PersistentGrantRepository.GetListAsync(
                input.SubjectId, input.Filter, input.Sorting,
                input.SkipCount, input.MaxResultCount);

            return new PagedResultDto<PersistedGrantDto>(persistenGrantCount,
                ObjectMapper.Map<List<PersistedGrant>, List<PersistedGrantDto>>(persistenGrants));
        }


    }
}
