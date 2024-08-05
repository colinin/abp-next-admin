using LINGYUN.Abp.IM.Contract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.MessageService.Chat;

[RemoteService(Name = AbpMessageServiceConsts.RemoteServiceName)]
[Route("api/im/my-friends")]
public class MyFriendController : AbpControllerBase, IMyFriendAppService
{
    protected IMyFriendAppService MyFriendAppService { get; }

    public MyFriendController(IMyFriendAppService myFriendAppService)
    {
        MyFriendAppService = myFriendAppService;
    }

    [HttpPost]
    public async virtual Task CreateAsync(MyFriendCreateDto input)
    {
        await MyFriendAppService.CreateAsync(input);
    }

    [HttpPost]
    [Route("add-request")]
    public async virtual Task AddRequestAsync(MyFriendAddRequestDto input)
    {
        await MyFriendAppService.AddRequestAsync(input);
    }

    [HttpDelete]
    public async virtual Task DeleteAsync(MyFriendOperationDto input)
    {
        await MyFriendAppService.DeleteAsync(input);
    }

    [HttpGet]
    [Route("{friendId}")]
    public async virtual Task<UserFriend> GetAsync(Guid friendId)
    {
        return await MyFriendAppService.GetAsync(friendId);
    }

    [HttpGet]
    [Route("all")]
    public async virtual Task<ListResultDto<UserFriend>> GetAllListAsync(GetMyFriendsDto input)
    {
        return await MyFriendAppService.GetAllListAsync(input);
    }

    [HttpGet]
    public async virtual Task<PagedResultDto<UserFriend>> GetListAsync(MyFriendGetByPagedDto input)
    {
        return await MyFriendAppService.GetListAsync(input);
    }
}
