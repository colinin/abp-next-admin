using LINGYUN.Abp.IM.Group;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.MessageService.Group
{
    public class UserGroupStore : DomainService, IUserGroupStore
    {
        private IObjectMapper _objectMapper;
        protected IObjectMapper ObjectMapper => LazyGetRequiredService(ref _objectMapper);

        private IUnitOfWorkManager _unitOfWorkManager;
        protected IUnitOfWorkManager UnitOfWorkManager => LazyGetRequiredService(ref _unitOfWorkManager);

        protected IUserChatGroupRepository  UserChatGroupRepository { get; }

        public UserGroupStore(
            IUserChatGroupRepository userChatGroupRepository)
        {
            UserChatGroupRepository = userChatGroupRepository;
        }

        public virtual async Task<bool> MemberHasInGroupAsync(Guid? tenantId, long groupId, Guid userId)
        {
            using (CurrentTenant.Change(tenantId))
            {
                return await UserChatGroupRepository.MemberHasInGroupAsync(groupId, userId);
            }
        }

        [UnitOfWork]
        public virtual async Task AddUserToGroupAsync(Guid? tenantId, Guid userId, long groupId, Guid acceptUserId)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                using (CurrentTenant.Change(tenantId))
                {
                    var userHasInGroup = await UserChatGroupRepository.MemberHasInGroupAsync(groupId, userId);
                    if (!userHasInGroup)
                    {
                        var userGroup = new UserChatGroup(groupId, userId, acceptUserId, tenantId);

                        await UserChatGroupRepository.InsertAsync(userGroup);

                        await unitOfWork.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task<GroupUserCard> GetUserGroupCardAsync(Guid? tenantId, long groupId, Guid userId)
        {
            using (CurrentTenant.Change(tenantId))
            {
                var groupUserCard = await UserChatGroupRepository.GetMemberAsync(groupId, userId);

                return groupUserCard;
            }
        }

        public async Task<IEnumerable<GroupUserCard>> GetMembersAsync(Guid? tenantId, long groupId)
        {
            using (CurrentTenant.Change(tenantId))
            {
                return await UserChatGroupRepository.GetMembersAsync(groupId);
            }
        }

        public async Task<IEnumerable<LINGYUN.Abp.IM.Group.Group>> GetUserGroupsAsync(Guid? tenantId, Guid userId)
        {
            using (CurrentTenant.Change(tenantId))
            {
                return await UserChatGroupRepository.GetMemberGroupsAsync(userId);
            }
        }

        [UnitOfWork]
        public async Task RemoveUserFormGroupAsync(Guid? tenantId, Guid userId, long groupId)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                using (CurrentTenant.Change(tenantId))
                {
                    await UserChatGroupRepository.RemoveMemberFormGroupAsync(groupId, userId);

                    await unitOfWork.SaveChangesAsync();
                }
            }
        }

        public async Task<int> GetMembersCountAsync(Guid? tenantId, long groupId)
        {
            using (CurrentTenant.Change(tenantId))
            {
                return await UserChatGroupRepository.GetMembersCountAsync(groupId);
            }
        }

        public async Task<List<GroupUserCard>> GetMembersAsync(
            Guid? tenantId, 
            long groupId,
            string sorting = nameof(GroupUserCard.UserId), 
            bool reverse = false, 
            int skipCount = 0, 
            int maxResultCount = 10)
        {
            using (CurrentTenant.Change(tenantId))
            {
                return await UserChatGroupRepository.GetMembersAsync(groupId, sorting, reverse, skipCount, maxResultCount);
            }
        }
    }
}
