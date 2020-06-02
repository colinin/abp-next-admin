using LINGYUN.Abp.IM.Group;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.MessageService.Messages
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

        Task<List<UserGroup>> GetGroupUsersAsync(long groupId);

        Task<List<Group>> GetUserGroupsAsync(Guid userId);
    }
}
