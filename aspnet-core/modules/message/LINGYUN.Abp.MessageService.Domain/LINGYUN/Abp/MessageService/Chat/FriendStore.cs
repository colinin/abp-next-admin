using LINGYUN.Abp.IM.Contract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class FriendStore : DomainService, IFriendStore
    {
        protected IDistributedCache<UserFriendCacheItem> Cache { get; }
        protected IUserChatFriendRepository UserChatFriendRepository { get; }
        protected IUserChatSettingRepository UserChatSettingRepository { get; }

        public FriendStore(
            IDistributedCache<UserFriendCacheItem> cache,
            IUserChatFriendRepository userChatFriendRepository,
            IUserChatSettingRepository userChatSettingRepository)
        {
            Cache = cache;
            UserChatFriendRepository = userChatFriendRepository;
            UserChatSettingRepository = userChatSettingRepository;
        }

        public virtual async Task<bool> IsFriendAsync(
            Guid? tenantId,
            Guid userId,
            Guid friendId
            )
        {
            using (CurrentTenant.Change(tenantId))
            {
                return await UserChatFriendRepository.IsAddedAsync(userId, friendId);
            }
        }

        [UnitOfWork]
        public virtual async Task<UserAddFriendResult> AddMemberAsync(Guid? tenantId, Guid userId, Guid friendId, string remarkName = "")
        {
            using (CurrentTenant.Change(tenantId))
            {
                if (await UserChatFriendRepository.IsAddedAsync(userId, friendId))
                {
                    throw new BusinessException(MessageServiceErrorCodes.UseHasBeenAddedTheFriendOrSendAuthorization);
                }

                var status = UserFriendStatus.NeedValidation;
                var userChatSetting = await UserChatSettingRepository.FindByUserIdAsync(friendId);
                if (userChatSetting != null)
                {
                    if (!userChatSetting.AllowAddFriend)
                    {
                        throw new BusinessException(MessageServiceErrorCodes.UseRefuseToAddFriend);
                    }

                    status = userChatSetting.RequireAddFriendValition
                        ? UserFriendStatus.NeedValidation
                        : UserFriendStatus.Added;
                }

                var userChatFriend = new UserChatFriend(userId, friendId, remarkName, status, tenantId)
                {
                    CreationTime = Clock.Now,
                    CreatorId = userId
                };

                await UserChatFriendRepository.InsertAsync(userChatFriend);

                return new UserAddFriendResult(status);
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
            return await GetAllFriendByCacheItemAsync(tenantId, userId, sorting, reverse);
        }

        public virtual async Task<int> GetCountAsync(Guid? tenantId, Guid userId, string filter = "")
        {
            using (CurrentTenant.Change(tenantId))
            {
                return await UserChatFriendRepository
                    .GetMembersCountAsync(userId, filter);
            }
        }

        public virtual async Task<List<UserFriend>> GetPagedListAsync(Guid? tenantId, Guid userId, string filter = "", string sorting = nameof(UserFriend.UserId), bool reverse = false, int skipCount = 0, int maxResultCount = 10)
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

        protected virtual async Task<List<UserFriend>> GetAllFriendByCacheItemAsync(
            Guid? tenantId,
            Guid userId,
            string sorting = nameof(UserFriend.UserId),
            bool reverse = false
            )
        {
            var cacheKey = UserFriendCacheItem.CalculateCacheKey(userId.ToString());
            Logger.LogDebug($"FriendStore.GetCacheItemAsync: {cacheKey}");

            var cacheItem = await Cache.GetAsync(cacheKey);
            if (cacheItem != null)
            {
                Logger.LogDebug($"Found in the cache: {cacheKey}");
                return cacheItem.Friends;
            }

            Logger.LogDebug($"Not found in the cache: {cacheKey}");
            using (CurrentTenant.Change(tenantId))
            {
                var friends = await UserChatFriendRepository
                    .GetAllMembersAsync(userId, sorting, reverse);
                cacheItem = new UserFriendCacheItem(friends);
                Logger.LogDebug($"Set item in the cache: {cacheKey}");
                await Cache.SetAsync(cacheKey, cacheItem);
                return friends;
            }
        }
    }
}
