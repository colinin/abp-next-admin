using LINGYUN.Abp.IM.Contract;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.MessageService.Chat
{
    public interface IMyFriendAppService : IApplicationService
    {
        Task<UserFriend> GetAsync(Guid friendId);

        Task<PagedResultDto<UserFriend>> GetListAsync(MyFriendGetByPagedDto input);

        Task<ListResultDto<UserFriend>> GetAllListAsync(GetMyFriendsDto input);

        Task CreateAsync(MyFriendCreateDto input);

        Task DeleteAsync(MyFriendOperationDto input);

        Task AddRequestAsync(MyFriendAddRequestDto input);
    }
}
