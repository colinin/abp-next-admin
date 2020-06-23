using LINGYUN.Abp.IM.Group;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.MessageService.Chat
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

        [UnitOfWork]
        public async Task AddUserToGroupAsync(Guid? tenantId, Guid userId, long groupId, Guid acceptUserId)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                using (CurrentTenant.Change(tenantId))
                {
                    var userHasInGroup = await UserChatGroupRepository.UserHasInGroupAsync(groupId, userId);
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
                var groupUserCard = await UserChatGroupRepository.GetGroupUserCardAsync(groupId, userId);

                return groupUserCard;
            }
        }

        public async Task<IEnumerable<UserGroup>> GetGroupUsersAsync(Guid? tenantId, long groupId)
        {
            using (CurrentTenant.Change(tenantId))
            {
                var userGroups = await UserChatGroupRepository.GetGroupUsersAsync(groupId);

                return userGroups;
            }
        }

        public async Task<IEnumerable<Group>> GetUserGroupsAsync(Guid? tenantId, Guid userId)
        {
            using (CurrentTenant.Change(tenantId))
            {
                var groups = await UserChatGroupRepository.GetUserGroupsAsync(userId);

                return groups;
            }
        }

        [UnitOfWork]
        public async Task RemoveUserFormGroupAsync(Guid? tenantId, Guid userId, long groupId)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                using (CurrentTenant.Change(tenantId))
                {
                    var userGroup = await UserChatGroupRepository.GetUserGroupAsync(groupId, userId);

                    if(userGroup != null)
                    {
                        await UserChatGroupRepository.DeleteAsync(userGroup);

                        await unitOfWork.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task<int> GetGroupUsersCountAsync(Guid? tenantId, long groupId, string filter = "")
        {
            using (CurrentTenant.Change(tenantId))
            {
                return await UserChatGroupRepository.GetGroupUsersCountAsync(groupId, filter);
            }
        }

        public async Task<List<UserGroup>> GetGroupUsersAsync(Guid? tenantId, long groupId, string filter = "", string sorting = "UserId", int skipCount = 1, int maxResultCount = 10)
        {
            using (CurrentTenant.Change(tenantId))
            {
                return await UserChatGroupRepository.GetGroupUsersAsync(groupId, filter, sorting, skipCount, maxResultCount);
            }
        }
    }
}
