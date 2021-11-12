using LINGYUN.Abp.IM.Groups;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.MessageService.Groups
{
    public class UserGroupStore : IUserGroupStore, ITransientDependency
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IUserChatGroupRepository _userChatGroupRepository;

        public UserGroupStore(
            ICurrentTenant currentTenant,
            IUnitOfWorkManager unitOfWorkManager,
            IUserChatGroupRepository userChatGroupRepository)
        {
            _currentTenant = currentTenant;
            _unitOfWorkManager = unitOfWorkManager;
            _userChatGroupRepository = userChatGroupRepository;
        }

        public virtual async Task<bool> MemberHasInGroupAsync(
            Guid? tenantId,
            long groupId,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                return await _userChatGroupRepository.MemberHasInGroupAsync(groupId, userId, cancellationToken);
            }
        }

        public virtual async Task AddUserToGroupAsync(
            Guid? tenantId,
            Guid userId,
            long groupId,
            Guid acceptUserId,
            CancellationToken cancellationToken = default)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                using (_currentTenant.Change(tenantId))
                {
                    var userHasInGroup = await _userChatGroupRepository.MemberHasInGroupAsync(groupId, userId, cancellationToken);
                    if (!userHasInGroup)
                    {
                        var userGroup = new UserChatGroup(groupId, userId, acceptUserId, tenantId);

                        await _userChatGroupRepository.InsertAsync(userGroup, cancellationToken: cancellationToken);

                        await unitOfWork.CompleteAsync(cancellationToken);
                    }
                }
            }
        }

        public async Task<GroupUserCard> GetUserGroupCardAsync(
            Guid? tenantId,
            long groupId,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                var groupUserCard = await _userChatGroupRepository.GetMemberAsync(groupId, userId, cancellationToken);

                return groupUserCard;
            }
        }

        public async Task<IEnumerable<GroupUserCard>> GetMembersAsync(
            Guid? tenantId,
            long groupId,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                return await _userChatGroupRepository.GetMembersAsync(groupId, cancellationToken: cancellationToken);
            }
        }

        public async Task<IEnumerable<Group>> GetUserGroupsAsync(
            Guid? tenantId,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                return await _userChatGroupRepository.GetMemberGroupsAsync(userId, cancellationToken);
            }
        }

        public async Task RemoveUserFormGroupAsync(
            Guid? tenantId,
            Guid userId,
            long groupId,
            CancellationToken cancellationToken = default)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                using (_currentTenant.Change(tenantId))
                {
                    await _userChatGroupRepository.RemoveMemberFormGroupAsync(groupId, userId, cancellationToken);

                    await unitOfWork.CompleteAsync(cancellationToken);
                }
            }
        }

        public async Task<int> GetMembersCountAsync(
            Guid? tenantId,
            long groupId,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                return await _userChatGroupRepository.GetMembersCountAsync(groupId, cancellationToken);
            }
        }

        public async Task<List<GroupUserCard>> GetMembersAsync(
            Guid? tenantId,
            long groupId,
            string sorting = nameof(GroupUserCard.UserId),
            int skipCount = 0,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default)
        {
            using (_currentTenant.Change(tenantId))
            {
                return await _userChatGroupRepository.GetMembersAsync(groupId, sorting, skipCount, maxResultCount, cancellationToken);
            }
        }
    }
}
