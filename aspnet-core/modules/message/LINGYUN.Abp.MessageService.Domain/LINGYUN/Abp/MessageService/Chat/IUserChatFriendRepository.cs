using LINGYUN.Abp.IM.Contract;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.MessageService.Chat
{
    public interface IUserChatFriendRepository : IBasicRepository<UserChatFriend, long>
    {
        Task<bool> IsFriendAsync(
            Guid userId,
            Guid frientId, 
            CancellationToken cancellationToken = default);

        Task<bool> IsAddedAsync(
            Guid userId,
            Guid frientId,
            CancellationToken cancellationToken = default);

        Task<UserChatFriend> FindByUserFriendIdAsync(
            Guid userId,
            Guid friendId,
            CancellationToken cancellationToken = default);

        Task<List<UserFriend>> GetAllMembersAsync(
            Guid userId,
            string sorting = nameof(UserChatFriend.RemarkName),
             CancellationToken cancellationToken = default);

        Task<int> GetMembersCountAsync(
            Guid userId,
            string filter = "",
             CancellationToken cancellationToken = default);

        Task<List<UserFriend>> GetMembersAsync(
            Guid userId,
            string filter = "",
            string sorting = nameof(UserChatFriend.UserId),
            int skipCount = 0,
            int maxResultCount = 10,
             CancellationToken cancellationToken = default);

        Task<List<UserFriend>> GetLastContactMembersAsync(
            Guid userId,
            int skipCount = 0,
            int maxResultCount = 10,
             CancellationToken cancellationToken = default);

        Task<UserFriend> GetMemberAsync(
            Guid userId,
            Guid friendId,
            CancellationToken cancellationToken = default);
    }
}
