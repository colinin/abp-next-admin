using LINGYUN.Abp.IM.Contract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class FriendStore : DomainService, IFriendStore
    {
        protected IUserChatFriendRepository UserChatFriendRepository { get; }

        public FriendStore(
            IUserChatFriendRepository userChatFriendRepository)
        {
            UserChatFriendRepository = userChatFriendRepository;
        }

        [UnitOfWork]
        public virtual async Task AddMemberAsync(Guid? tenantId, Guid userId, Guid friendId, string remarkName = "")
        {
            using (CurrentTenant.Change(tenantId))
            {
                if (await UserChatFriendRepository.IsAddedAsync(userId, friendId))
                {
                    throw new BusinessException(MessageServiceErrorCodes.YouHaveAddedTheUserToFriend);
                    
                }
                var userChatFriend = new UserChatFriend(userId, friendId, remarkName, tenantId)
                {
                    CreationTime = Clock.Now,
                    CreatorId = userId
                };

                await UserChatFriendRepository.InsertAsync(userChatFriend);
            }
        }

        [UnitOfWork]
        public virtual async Task AddShieldMemberAsync(Guid? tenantId, Guid userId, Guid friendId)
        {
            await ChangeFriendShieldAsync(tenantId, userId, friendId, true);
        }

        public virtual async Task<List<UserFriend>> GetListAsync(
            Guid? tenantId,
            Guid userId,
            string sorting = nameof(UserFriend.UserId),
            bool reverse = false
            )
        {
            using (CurrentTenant.Change(tenantId))
            {
                return await UserChatFriendRepository
                    .GetAllMembersAsync(userId, sorting, reverse);
            }
        }

        public virtual async Task<int> GetCountAsync(Guid? tenantId, Guid userId, string filter = "")
        {
            using (CurrentTenant.Change(tenantId))
            {
                return await UserChatFriendRepository
                    .GetMembersCountAsync(userId, filter);
            }
        }

        public virtual async Task<List<UserFriend>> GetListAsync(Guid? tenantId, Guid userId, string filter = "", string sorting = nameof(UserFriend.UserId), bool reverse = false, int skipCount = 0, int maxResultCount = 10)
        {
            using (CurrentTenant.Change(tenantId))
            {
                return await UserChatFriendRepository
                    .GetMembersAsync(userId, filter, sorting, reverse,
                        skipCount, maxResultCount);
            }
        }

        public virtual async Task<List<UserFriend>> GetLastContactListAsync(
            Guid? tenantId,
            Guid userId,
            int skipCount = 0,
            int maxResultCount = 10)
        {
            using (CurrentTenant.Change(tenantId))
            {
                return await UserChatFriendRepository
                    .GetLastContactMembersAsync(userId,
                        skipCount, maxResultCount);
            }
        }

        public virtual async Task<UserFriend> GetMemberAsync(Guid? tenantId, Guid userId, Guid friendId)
        {
            using (CurrentTenant.Change(tenantId))
            {
                return await UserChatFriendRepository
                    .GetMemberAsync(userId, friendId);
            }
        }

        [UnitOfWork]
        public virtual async Task RemoveMemberAsync(Guid? tenantId, Guid userId, Guid friendId)
        {
            using (CurrentTenant.Change(tenantId))
            {
                var userChatFriend = await UserChatFriendRepository.FindByUserFriendIdAsync(userId, friendId);
                if (userChatFriend != null)
                {
                    await UserChatFriendRepository.DeleteAsync(userChatFriend);
                }
            }
        }

        [UnitOfWork]
        public virtual async Task RemoveShieldMemberAsync(Guid? tenantId, Guid userId, Guid friendId)
        {
            await ChangeFriendShieldAsync(tenantId, userId, friendId, false);
        }


        protected virtual async Task ChangeFriendShieldAsync(Guid? tenantId, Guid userId, Guid friendId, bool isBlack = false)
        {
            using (CurrentTenant.Change(tenantId))
            {
                var userChatFriend = await UserChatFriendRepository.FindByUserFriendIdAsync(userId, friendId);
                if (userChatFriend != null)
                {
                    userChatFriend.Black = isBlack;
                    await UserChatFriendRepository.UpdateAsync(userChatFriend);
                }
            }
        }
    }
}
