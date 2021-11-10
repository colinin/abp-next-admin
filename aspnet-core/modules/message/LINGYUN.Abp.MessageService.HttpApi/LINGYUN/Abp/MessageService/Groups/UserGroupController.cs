using LINGYUN.Abp.IM.Groups;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.MessageService.Groups
{
    [RemoteService(Name = AbpMessageServiceConsts.RemoteServiceName)]
    [Route("api/im/user-groups")]
    public class UserGroupController : AbpController, IUserGroupAppService
    {
        private readonly IUserGroupAppService _service;

        public UserGroupController(IUserGroupAppService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("join")]
        public virtual async Task ApplyJoinGroupAsync(UserJoinGroupDto input)
        {
            await _service.ApplyJoinGroupAsync(input);
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<GroupUserCard>> GetGroupUsersAsync(GroupUserGetByPagedDto input)
        {
            return await _service.GetGroupUsersAsync(input);
        }

        [HttpGet]
        [Route("me")]
        public virtual async Task<ListResultDto<Group>> GetMyGroupsAsync()
        {
            return await _service.GetMyGroupsAsync();
        }

        [HttpPut]
        [Route("accept")]
        public virtual async Task GroupAcceptUserAsync(GroupAcceptUserDto input)
        {
            await _service.GroupAcceptUserAsync(input);
        }

        [HttpPut]
        [Route("remove")]
        public virtual async Task GroupRemoveUserAsync(GroupRemoveUserDto input)
        {
            await _service.GroupRemoveUserAsync(input);
        }
    }
}
