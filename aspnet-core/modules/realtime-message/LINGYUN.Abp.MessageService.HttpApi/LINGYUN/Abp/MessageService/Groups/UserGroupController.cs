using LINGYUN.Abp.IM.Groups;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.MessageService.Groups;

[RemoteService(Name = AbpMessageServiceConsts.RemoteServiceName)]
[Route("api/im/user-groups")]
public class UserGroupController : AbpControllerBase, IUserGroupAppService
{
    private readonly IUserGroupAppService _service;

    public UserGroupController(IUserGroupAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("join")]
    public async virtual Task ApplyJoinGroupAsync(UserJoinGroupDto input)
    {
        await _service.ApplyJoinGroupAsync(input);
    }

    [HttpGet]
    public async virtual Task<PagedResultDto<GroupUserCard>> GetGroupUsersAsync(GroupUserGetByPagedDto input)
    {
        return await _service.GetGroupUsersAsync(input);
    }

    [HttpGet]
    [Route("me")]
    public async virtual Task<ListResultDto<Group>> GetMyGroupsAsync()
    {
        return await _service.GetMyGroupsAsync();
    }

    [HttpPut]
    [Route("accept")]
    public async virtual Task GroupAcceptUserAsync(GroupAcceptUserDto input)
    {
        await _service.GroupAcceptUserAsync(input);
    }

    [HttpPut]
    [Route("remove")]
    public async virtual Task GroupRemoveUserAsync(GroupRemoveUserDto input)
    {
        await _service.GroupRemoveUserAsync(input);
    }
}
