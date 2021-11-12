using LINGYUN.Abp.IM.Contract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class FriendStore : IFriendStore, ITransientDependency
    {
        private readonly IClock _clock;
        private readonly ILogger _logger;
        private readonly ICurrentTenant _currentTenant;
        private readonly IDistributedCache<UserFriendCacheItem> _cache;
        private readonly IUserChatFriendRepository _userChatFriendRepository;
        private readonly IUserChatSettingRepository _userChatSettingRepository;

        public FriendStore(
            IClock clock,
            ILogger<FriendStore> logger,
            ICurrentTenant currentTenant,
            IDistributedCache<UserFriendCacheItem> cache,
            IUserChatFriendRepository userChatFriendRepository,
            IUserChatSettingRepository userChatSettingRepository
            )
        {
            _clock = clock;
            _cache = cache;
            _logger = logger;
            _currentTenant = currentTenant;
            _userChatFriendRepository = userChatFriendRepository;
            _userChatSettingRepository = userChatSettingRepository;
        }

        public virtual async Task<bool> IsFriendAsync(
            Guid? tenantId,
            Guid userId,
            Guid friendId,
            CancellationToken cancellationToken = default
            )
        {
            using (_currentTenant.Change(tenantId))
            {
                return await _userChatFriendRepository.IsFriendAsync(userId, friendId, cancellationToken);
            }
        }

        [UnitOfWork]
        public virtual async Task AddMemberAsync(
            Guid? tenantId,
            Guid userId,
            Guid friendId,
            string remarkName = "",
            bool isStatic = false,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                if (!await _userChatFriendRepository.IsAddedAsync(userId, friendId))
                {
                    var userFriend = new UserChatFriend(userId, friendId, remarkName);
                    userFriend.SetStatus(UserFriendStatus.Added);
                    userFriend.IsStatic = isStatic;

                    await _userChatFriendRepository.InsertAsync(userFriend);
                }

                var userChatFriend = await _userChatFriendRepository
                    .FindByUserFriendIdAsync(friendId, userId);

                userChatFriend.SetStatus(UserFriendStatus.Added);

                await _userChatFriendRepository.UpdateAsync(userChatFriend, cancellationToken: cancellationToken);
            }
        }

        [UnitOfWork]
        public virtual async Task<UserAddFriendResult> AddRequestAsync(
            Guid? tenantId,
            Guid userId,
            Guid friendId,
            string remarkName = "",
            string description = "",
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                if (await _userChatFriendRepository.IsAddedAsync(userId, friendId))
                {
                    throw new BusinessException(MessageServiceErrorCodes.UseHasBeenAddedTheFriendOrSendAuthorization);
                }

                var status = UserFriendStatus.NeedValidation;
                var userChatSetting = await _userChatSettingRepository.FindByUserIdAsync(friendId, cancellationToken);
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

                var userChatFriend = new UserChatFriend(userId, friendId, remarkName, description, tenantId)
                {
                    CreationTime = _clock.Now,
                    CreatorId = userId,
                };
                userChatFriend.SetStatus(status);

                await _userChatFriendRepository.InsertAsync(userChatFriend, cancellationToken: cancellationToken);

                return new UserAddFriendResult(status);
            }
        }

        [UnitOfWork]
        public virtual async Task AddShieldMemberAsync(
            Guid? tenantId,
            Guid userId,
            Guid friendId,
            CancellationToken cancellationToken = default)
        {
            await ChangeFriendShieldAsync(tenantId, userId, friendId, true, cancellationToken);
        }

        public virtual async Task<List<UserFriend>> GetListAsync(
            Guid? tenantId,
            Guid userId,
            string sorting = nameof(UserFriend.UserId),
            CancellationToken cancellationToken = default
            )
        {
            using (_currentTenant.Change(tenantId))
            {
                return await GetAllFriendByCacheItemAsync(userId, sorting, cancellationToken);
            }
        }

        public virtual async Task<int> GetCountAsync(
            Guid? tenantId,
            Guid userId,
            string filter = "",
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                return await _userChatFriendRepository
                    .GetMembersCountAsync(userId, filter, cancellationToken);
            }
        }

        public virtual async Task<List<UserFriend>> GetPagedListAsync(
            Guid? tenantId,
            Guid userId,
            string filter = "",
            string sorting = nameof(UserFriend.UserId),
            int skipCount = 0,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                return await _userChatFriendRepository
                    .GetMembersAsync(userId, filter, sorting,
                        skipCount, maxResultCount, cancellationToken);
            }
        }

        public virtual async Task<List<UserFriend>> GetLastContactListAsync(
            Guid? tenantId,
            Guid userId,
            int skipCount = 0,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                return await _userChatFriendRepository
                    .GetLastContactMembersAsync(userId,
                        skipCount, maxResultCount, cancellationToken);
            }
        }

        public virtual async Task<UserFriend> GetMemberAsync(
            Guid? tenantId,
            Guid userId,
            Guid friendId,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                return await _userChatFriendRepository
                    .GetMemberAsync(userId, friendId, cancellationToken);
            }
        }

        [UnitOfWork]
        public virtual async Task RemoveMemberAsync(
            Guid? tenantId,
            Guid userId,
            Guid friendId,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                var userChatFriend = await _userChatFriendRepository.FindByUserFriendIdAsync(userId, friendId, cancellationToken);
                if (userChatFriend != null)
                {
                    await _userChatFriendRepository.DeleteAsync(userChatFriend, cancellationToken: cancellationToken);
                }
            }
        }

        [UnitOfWork]
        public virtual async Task RemoveShieldMemberAsync(
            Guid? tenantId,
            Guid userId,
            Guid friendId,
            CancellationToken cancellationToken = default)
        {
            await ChangeFriendShieldAsync(tenantId, userId, friendId, false, cancellationToken);
        }


        protected virtual async Task ChangeFriendShieldAsync(
            Guid? tenantId,
            Guid userId,
            Guid friendId,
            bool isBlack = false,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                var userChatFriend = await _userChatFriendRepository.FindByUserFriendIdAsync(userId, friendId, cancellationToken);
                if (userChatFriend != null)
                {
                    userChatFriend.Black = isBlack;
                    await _userChatFriendRepository.UpdateAsync(userChatFriend, cancellationToken: cancellationToken);
                }
            }
        }

        protected virtual async Task<List<UserFriend>> GetAllFriendByCacheItemAsync(
            Guid userId,
            string sorting = nameof(UserFriend.UserId),
            CancellationToken cancellationToken = default
            )
        {
            var cacheKey = UserFriendCacheItem.CalculateCacheKey(userId.ToString());
            _logger.LogDebug($"FriendStore.GetCacheItemAsync: {cacheKey}");

            var cacheItem = await _cache.GetAsync(cacheKey, token: cancellationToken);
            if (cacheItem != null)
            {
                _logger.LogDebug($"Found in the cache: {cacheKey}");
                return cacheItem.Friends;
            }

            _logger.LogDebug($"Not found in the cache: {cacheKey}");
            var friends = await _userChatFriendRepository
                    .GetAllMembersAsync(userId, sorting, cancellationToken);
            cacheItem = new UserFriendCacheItem(friends);
            _logger.LogDebug($"Set item in the cache: {cacheKey}");
            await _cache.SetAsync(cacheKey, cacheItem, token: cancellationToken);
            return friends;
        }
    }
}
