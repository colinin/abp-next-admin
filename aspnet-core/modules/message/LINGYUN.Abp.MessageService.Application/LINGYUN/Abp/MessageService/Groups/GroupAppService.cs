using LINGYUN.Abp.IM.Groups;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.MessageService.Groups
{
    [AllowAnonymous]
    public class GroupAppService : AbpMessageServiceApplicationServiceBase, IGroupAppService
    {
        private readonly IGroupStore _groupStore;

        public GroupAppService(
            IGroupStore groupStore)
        {
            _groupStore = groupStore;
        }

        public virtual async Task<Group> GetAsync(string groupId)
        {
            return await _groupStore.GetAsync(CurrentTenant.Id, groupId);
        }

        public virtual async Task<PagedResultDto<Group>> SearchAsync(GroupSearchInput input)
        {
            var count = await _groupStore.GetCountAsync(
                CurrentTenant.Id,
                input.Filter);

            var groups = await _groupStore.GetListAsync(
                CurrentTenant.Id,
                input.Filter,
                input.Sorting,
                input.SkipCount,
                input.MaxResultCount);

            return new PagedResultDto<Group>(count, groups);
        }
    }
}
