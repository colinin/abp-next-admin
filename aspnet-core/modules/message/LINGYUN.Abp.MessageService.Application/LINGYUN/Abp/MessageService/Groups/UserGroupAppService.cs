using LINGYUN.Abp.IM.Groups;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace LINGYUN.Abp.MessageService.Groups
{
    [Authorize]
    public class UserGroupAppService : AbpMessageServiceApplicationServiceBase, IUserGroupAppService
    {
        private readonly IUserGroupStore _userGroupStore;

        public UserGroupAppService(
            IUserGroupStore userGroupStore)
        {
            _userGroupStore = userGroupStore;
        }

        public virtual Task ApplyJoinGroupAsync(UserJoinGroupDto input)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<PagedResultDto<GroupUserCard>> GetGroupUsersAsync(GroupUserGetByPagedDto input)
        {
            var groupUserCardCount = await _userGroupStore
                .GetMembersCountAsync(CurrentTenant.Id, input.GroupId);

            var groupUserCards = await _userGroupStore.GetMembersAsync(
                CurrentTenant.Id,
                input.GroupId,
                input.Sorting,
                input.SkipCount,
                input.MaxResultCount);

            return new PagedResultDto<GroupUserCard>(groupUserCardCount, groupUserCards);
        }

        public virtual async Task<ListResultDto<Group>> GetMyGroupsAsync()
        {
            var myGroups = await _userGroupStore.GetUserGroupsAsync(CurrentTenant.Id, CurrentUser.GetId());

            return new ListResultDto<Group>(myGroups.ToImmutableList());
        }

        public virtual async Task GroupAcceptUserAsync(GroupAcceptUserDto input)
        {
            var myGroupCard = await _userGroupStore
                .GetUserGroupCardAsync(CurrentTenant.Id, input.GroupId, CurrentUser.GetId());
            if (myGroupCard == null)
            {
                // 当前登录用户不再用户组
                throw new UserFriendlyException("");
            }
            if (!myGroupCard.IsAdmin)
            {
                // 当前登录用户没有加人权限
                throw new UserFriendlyException("");
            }
            await _userGroupStore
                .AddUserToGroupAsync(CurrentTenant.Id, input.UserId, input.GroupId, CurrentUser.GetId());
        }

        public virtual async Task GroupRemoveUserAsync(GroupRemoveUserDto input)
        {
            var myGroupCard = await _userGroupStore
                .GetUserGroupCardAsync(CurrentTenant.Id, input.GroupId, CurrentUser.GetId());
            if (myGroupCard == null)
            {
                // 当前登录用户不再用户组
                throw new UserFriendlyException("");
            }
            if (!myGroupCard.IsAdmin)
            {
                // 当前登录用户没有踢人权限
                throw new UserFriendlyException("");
            }
            await _userGroupStore
                .RemoveUserFormGroupAsync(CurrentTenant.Id, input.UserId, input.GroupId);
        }
    }
}
