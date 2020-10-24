using LINGYUN.Abp.IM.Contract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.MessageService.Chat
{
    [RemoteService(Name = AbpMessageServiceConsts.RemoteServiceName)]
    [Route("api/im/my-friends")]
    public class MyFriendController : AbpController, IMyFriendAppService
    {
        protected IMyFriendAppService MyFriendAppService { get; }

        public MyFriendController(IMyFriendAppService myFriendAppService)
        {
            MyFriendAppService = myFriendAppService;
        }

        [HttpPost]
        public virtual async Task CreateAsync(MyFriendCreateDto input)
        {
            await MyFriendAppService.CreateAsync(input);
        }

        [HttpDelete]
        public virtual async Task DeleteAsync(MyFriendOperationDto input)
        {
            await MyFriendAppService.DeleteAsync(input);
        }

        [HttpGet]
        [Route("last-contacts")]
        public virtual async Task<PagedResultDto<UserFriend>> GetLastContactFriendsAsync(PagedResultRequestDto input)
        {
            return await MyFriendAppService.GetLastContactFriendsAsync(input);
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<UserFriend>> GetMyFriendsAsync(MyFriendGetByPagedDto input)
        {
            return await MyFriendAppService.GetMyFriendsAsync(input);
        }
    }
}
