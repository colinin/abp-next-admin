using LINGYUN.Abp.IM;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class UserCardFinder : DomainService, IUserCardFinder
    {
        protected IUserChatCardRepository UserChatCardRepository { get; }

        public UserCardFinder(
            IUserChatCardRepository userChatCardRepository)
        {
            UserChatCardRepository = userChatCardRepository;
        }

        public virtual async Task<int> GetCountAsync(Guid? tenantId, string findUserName = "", int? startAge = null, int? endAge = null, Sex? sex = null)
        {
            using (CurrentTenant.Change(tenantId))
            {
                return await UserChatCardRepository
                    .GetMemberCountAsync(findUserName, startAge, endAge, sex);
            }
        }

        public virtual async Task<List<UserCard>> GetListAsync(Guid? tenantId, string findUserName = "", int? startAge = null, int? endAge = null, Sex? sex = null, string sorting = nameof(UserCard.UserId), bool reverse = false, int skipCount = 0, int maxResultCount = 10)
        {
            using (CurrentTenant.Change(tenantId))
            {
                return await UserChatCardRepository
                    .GetMembersAsync(findUserName, startAge, endAge, sex,
                        sorting, reverse, skipCount, maxResultCount);
            }
        }

        public virtual async Task<UserCard> GetMemberAsync(Guid? tenantId, Guid findUserId)
        {
            using (CurrentTenant.Change(tenantId))
            {
                return await UserChatCardRepository
                    .GetMemberAsync(findUserId);
            }
        }
    }
}
