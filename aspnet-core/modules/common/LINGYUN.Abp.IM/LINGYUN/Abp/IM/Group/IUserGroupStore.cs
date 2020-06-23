using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.IM.Group
{
    public interface IUserGroupStore
    {
        /// <summary>
        /// 获取群组用户身份
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="groupId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<GroupUserCard> GetUserGroupCardAsync(Guid? tenantId, long groupId, Guid userId);
        /// <summary>
        /// 获取用户所在通讯组列表
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<Group>> GetUserGroupsAsync(Guid? tenantId, Guid userId);
        /// <summary>
        /// 获取通讯组所有用户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<IEnumerable<UserGroup>> GetGroupUsersAsync(Guid? tenantId, long groupId);
        /// <summary>
        /// 获取通讯组用户数
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="groupId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<int> GetGroupUsersCountAsync(Guid? tenantId, long groupId, string filter = "");
        /// <summary>
        /// 获取通讯组用户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="groupId"></param>
        /// <param name="filter"></param>
        /// <param name="sorting"></param>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <returns></returns>
        Task<List<UserGroup>> GetGroupUsersAsync(Guid? tenantId, long groupId, string filter = "", string sorting = nameof(UserGroup.UserId), int skipCount = 1, int maxResultCount = 10);
        /// <summary>
        /// 用户加入通讯组
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task AddUserToGroupAsync(Guid? tenantId, Guid userId, long groupId, Guid acceptUserId);
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
