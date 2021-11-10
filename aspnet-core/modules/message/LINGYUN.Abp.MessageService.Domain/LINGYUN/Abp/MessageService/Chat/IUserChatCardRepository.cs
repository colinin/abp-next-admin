using LINGYUN.Abp.IM;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.MessageService.Chat
{
    public interface IUserChatCardRepository : IBasicRepository<UserChatCard, long>
    {
        Task<UserChatCard> FindByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<bool> CheckUserIdExistsAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<int> GetMemberCountAsync(
            string findUserName = "",
            int? startAge = null,
            int? endAge = null,
            Sex? sex = null,
            CancellationToken cancellationToken = default);

        Task<List<UserCard>> GetMembersAsync(
            string findUserName = "",
            int? startAge = null,
            int? endAge = null,
            Sex? sex = null,
            string sorting = nameof(UserChatCard.UserId),
            int skipCount = 0,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default);

        Task<UserCard> GetMemberAsync(
            Guid findUserId,
            CancellationToken cancellationToken = default);
    }
}
