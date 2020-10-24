using LINGYUN.Abp.IM.Contract;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.MessageService.Chat
{
    public interface IMyFriendAppService : IApplicationService
    {
        Task<PagedResultDto<UserFriend>> GetMyFriendsAsync(MyFriendGetByPagedDto input);

        Task<PagedResultDto<UserFriend>> GetLastContactFriendsAsync(PagedResultRequestDto input);

        Task CreateAsync(MyFriendCreateDto input);

        Task DeleteAsync(MyFriendOperationDto input);
    }
}
