using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.IM.Groups
{
    public interface IUserGroupStore
    {
        /// <summary>
        /// 成员是否在群组
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="groupId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> MemberHasInGroupAsync(
            Guid? tenantId,
            long groupId,
            Guid userId,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取群组用户身份
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="groupId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<GroupUserCard> GetUserGroupCardAsync(
            Guid? tenantId,
            long groupId,
            Guid userId,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取用户所在通讯组列表
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<Group>> GetUserGroupsAsync(
            Guid? tenantId,
            Guid userId,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取群组成员列表
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<IEnumerable<GroupUserCard>> GetMembersAsync(
            Guid? tenantId,
            long groupId,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取群组成员数
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<int> GetMembersCountAsync(
            Guid? tenantId,
            long groupId,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取通讯组用户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="groupId"></param>
        /// <param name="sorting"></param>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <returns></returns>
        Task<List<GroupUserCard>> GetMembersAsync(
            Guid? tenantId,
            long groupId,
            string sorting = nameof(GroupUserCard.UserId),
            int skipCount = 0,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 用户加入通讯组
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task AddUserToGroupAsync(
            Guid? tenantId,
            Guid userId,
            long groupId,
            Guid acceptUserId,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 用户退出通讯组
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task RemoveUserFormGroupAsync(
            Guid? tenantId,
            Guid userId,
            long groupId,
            CancellationToken cancellationToken = default);
    }
}
