using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.IM.Group
{
    public interface IUserGroupStore
    {
        /// <summary>
        /// 获取用户所在通讯组列表
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<GroupChat>> GetUserGroupsAsync(Guid? tenantId, Guid userId);
        /// <summary>
        /// 获取通讯组所有用户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<IEnumerable<UserGroup>> GetGroupUsersAsync(Guid? tenantId, long groupId);
        /// <summary>
        /// 用户加入通讯组
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task AddUserToGroupAsync(Guid? tenantId, Guid userId, long groupId);
        /// <summary>
        /// 用户退出通讯组
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task RemoveUserFormGroupAsync(Guid? tenantId, Guid userId, long groupId);
    }
}
