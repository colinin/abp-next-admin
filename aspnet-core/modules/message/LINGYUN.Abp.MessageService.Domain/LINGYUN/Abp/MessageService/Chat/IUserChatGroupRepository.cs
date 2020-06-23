using LINGYUN.Abp.IM.Group;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.MessageService.Chat
{
    public interface IUserChatGroupRepository : IBasicRepository<UserChatGroup, long>
    {
        /// <summary>
        /// 用户是否在组里
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> UserHasInGroupAsync(long groupId, Guid userId);

        Task<UserChatGroup> GetUserGroupAsync(long groupId, Guid userId);

        Task<GroupUserCard> GetGroupUserCardAsync(long groupId, Guid userId);

        Task<List<UserGroup>> GetGroupUsersAsync(long groupId);

        Task<int> GetGroupUsersCountAsync(long groupId, string filter = "");

        Task<List<UserGroup>> GetGroupUsersAsync(long groupId, string filter = "", string sorting = nameof(UserGroup.UserId), int skipCount = 1, int maxResultCount = 10);

        Task<List<Group>> GetUserGroupsAsync(Guid userId);
    }
}
