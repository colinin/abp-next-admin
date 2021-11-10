using LINGYUN.Abp.IM.Groups;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.MessageService.Groups
{
    public interface IUserChatGroupRepository : IBasicRepository<UserChatGroup, long>
    {
        /// <summary>
        /// 成员是否在群组里
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> MemberHasInGroupAsync(
            long groupId,
            Guid userId,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取群组成员
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<GroupUserCard> GetMemberAsync(
            long groupId,
            Guid userId,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取群组成员数
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> GetMembersCountAsync(
            long groupId,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取群组成员列表
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="sorting"></param>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<GroupUserCard>> GetMembersAsync(
            long groupId,
            string sorting = nameof(GroupUserCard.UserId),
            int skipCount = 0,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取成员所在群组列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<Group>> GetMemberGroupsAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 从群组中移除成员
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RemoveMemberFormGroupAsync(
            long groupId,
            Guid userId,
            CancellationToken cancellationToken = default);
    }
}
