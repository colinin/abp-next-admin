using LINGYUN.Abp.IM;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class UserCardFinder : IUserCardFinder, ITransientDependency
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly IUserChatCardRepository _userChatCardRepository;

        public UserCardFinder(
            ICurrentTenant currentTenant,
            IUserChatCardRepository userChatCardRepository)
        {
            _currentTenant = currentTenant;
            _userChatCardRepository = userChatCardRepository;
        }

        public virtual async Task<int> GetCountAsync(
            Guid? tenantId,
            string findUserName = "", 
            int? startAge = null, 
            int? endAge = null, 
            Sex? sex = null)
        {
            using (_currentTenant.Change(tenantId))
            {
                return await _userChatCardRepository
                    .GetMemberCountAsync(findUserName, startAge, endAge, sex);
            }
        }

        public virtual async Task<List<UserCard>> GetListAsync(
            Guid? tenantId, 
            string findUserName = "", 
            int? startAge = null, 
            int? endAge = null,
            Sex? sex = null, 
            string sorting = nameof(UserCard.UserId), 
            int skipCount = 0,
            int maxResultCount = 10)
        {
            using (_currentTenant.Change(tenantId))
            {
                return await _userChatCardRepository
                    .GetMembersAsync(findUserName, startAge, endAge, sex,
                        sorting, skipCount, maxResultCount);
            }
        }

        public virtual async Task<UserCard> GetMemberAsync(Guid? tenantId, Guid findUserId)
        {
            using (_currentTenant.Change(tenantId))
            {
                return await _userChatCardRepository
                    .GetMemberAsync(findUserId);
            }
        }
    }
}
