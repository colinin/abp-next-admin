using LINGYUN.Abp.IM.Contract;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace LINGYUN.Abp.MessageService.Chat
{
    [Authorize]
    public class MyFriendAppService : ApplicationService, IMyFriendAppService
    {
        protected IFriendStore FriendStore { get; }

        public MyFriendAppService(IFriendStore friendStore)
        {
            FriendStore = friendStore;
        }

        public virtual async Task CreateAsync(MyFriendCreateDto input)
        {
            await FriendStore.AddMemberAsync(CurrentTenant.Id, CurrentUser.GetId(), input.FriendId, input.RemarkName);
        }

        public virtual async Task DeleteAsync(MyFriendOperationDto input)
        {
            await FriendStore.RemoveMemberAsync(CurrentTenant.Id, CurrentUser.GetId(), input.FriendId);
        }

        public virtual async Task<PagedResultDto<UserFriend>> GetLastContactFriendsAsync(PagedResultRequestDto input)
        {
            var myFrientCount = await FriendStore.GetCountAsync(CurrentTenant.Id, CurrentUser.GetId());

            var myFriends = await FriendStore
                .GetLastContactListAsync(CurrentTenant.Id, CurrentUser.GetId(),
                    input.SkipCount, input.MaxResultCount);

            return new PagedResultDto<UserFriend>(myFrientCount, myFriends);
        }

        public virtual async Task<PagedResultDto<UserFriend>> GetMyFriendsAsync(MyFriendGetByPagedDto input)
        {
            var myFrientCount = await FriendStore.GetCountAsync(CurrentTenant.Id, CurrentUser.GetId());

            var myFriends = await FriendStore
                .GetListAsync(CurrentTenant.Id, CurrentUser.GetId(),
                    input.Filter, input.Sorting, input.Reverse, 
                    input.SkipCount, input.MaxResultCount);

            return new PagedResultDto<UserFriend>(myFrientCount, myFriends);
        }
    }
}
