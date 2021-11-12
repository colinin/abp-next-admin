using LINGYUN.Abp.IM.Contract;
using LINGYUN.Abp.MessageService.Localization;
using Microsoft.AspNetCore.Authorization;
using System;
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

        protected IUserChatCardRepository UserChatCardRepository { get; }

        public MyFriendAppService(
            IFriendStore friendStore,
            IUserChatCardRepository userChatCardRepository)
        {
            FriendStore = friendStore;
            UserChatCardRepository = userChatCardRepository;

            LocalizationResource = typeof(MessageServiceResource);
        }

        public virtual async Task<UserFriend> GetAsync(Guid friendId)
        {
            return await FriendStore.GetMemberAsync(CurrentTenant.Id, CurrentUser.GetId(), friendId);
        }

        public virtual async Task CreateAsync(MyFriendCreateDto input)
        {
            var friendCard = await UserChatCardRepository.GetMemberAsync(input.FriendId);

            await FriendStore.AddMemberAsync(
                CurrentTenant.Id,
                CurrentUser.GetId(),
                input.FriendId, friendCard?.NickName ?? friendCard?.UserName ?? input.FriendId.ToString());
        }

        public virtual async Task AddRequestAsync(MyFriendAddRequestDto input)
        {
            await FriendStore.AddRequestAsync(CurrentTenant.Id, CurrentUser.GetId(), input.FriendId, input.RemarkName, L["AddNewFriendBySearchId"]);
        }

        public virtual async Task DeleteAsync(MyFriendOperationDto input)
        {
            await FriendStore.RemoveMemberAsync(CurrentTenant.Id, CurrentUser.GetId(), input.FriendId);
        }

        public virtual async Task<ListResultDto<UserFriend>> GetAllListAsync(GetMyFriendsDto input)
        {
            var myFriends = await FriendStore
                .GetListAsync(
                    CurrentTenant.Id,
                    CurrentUser.GetId(),
                    input.Sorting);

            return new ListResultDto<UserFriend>(myFriends);
        }

        public virtual async Task<PagedResultDto<UserFriend>> GetListAsync(MyFriendGetByPagedDto input)
        {
            var myFrientCount = await FriendStore.GetCountAsync(CurrentTenant.Id, CurrentUser.GetId());

            var myFriends = await FriendStore
                .GetPagedListAsync(CurrentTenant.Id, CurrentUser.GetId(),
                    input.Filter, input.Sorting,
                    input.SkipCount, input.MaxResultCount);

            return new PagedResultDto<UserFriend>(myFrientCount, myFriends);
        }
    }
}
