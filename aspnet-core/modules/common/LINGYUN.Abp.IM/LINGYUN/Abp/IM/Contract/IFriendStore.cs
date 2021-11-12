using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.IM.Contract
{
    public interface IFriendStore
    {
        /// <summary>
        /// 是否是好友关系
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        Task<bool> IsFriendAsync(
            Guid? tenantId,
            Guid userId,
            Guid friendId,
            CancellationToken cancellationToken = default
            );
        /// <summary>
        /// 查询好友列表
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="sorting"></param>
        /// <returns></returns>
        Task<List<UserFriend>> GetListAsync(
            Guid? tenantId,
            Guid userId,
            string sorting = nameof(UserFriend.UserId),
            CancellationToken cancellationToken = default
            );
        /// <summary>
        /// 获取好友数量
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<int> GetCountAsync(
            Guid? tenantId,
            Guid userId,
            string filter = "",
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="filter"></param>
        /// <param name="sorting"></param>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <returns></returns>
        Task<List<UserFriend>> GetPagedListAsync(
            Guid? tenantId,
            Guid userId,
            string filter = "",
            string sorting = nameof(UserFriend.UserId),
            int skipCount = 0,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取最近联系好友列表
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <returns></returns>
        Task<List<UserFriend>> GetLastContactListAsync(
            Guid? tenantId,
            Guid userId,
            int skipCount = 0,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取好友信息
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        Task<UserFriend> GetMemberAsync(
            Guid? tenantId,
            Guid userId,
            Guid friendId,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        Task AddMemberAsync(
            Guid? tenantId,
            Guid userId,
            Guid friendId,
            string remarkName = "",
            bool isStatic = false,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 添加好友请求
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="friendId"></param>
        /// <param name="remarkName"></param>
        /// <returns></returns>
        Task<UserAddFriendResult> AddRequestAsync(
            Guid? tenantId,
            Guid userId,
            Guid friendId,
            string remarkName = "",
            string description = "",
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 移除好友
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        Task RemoveMemberAsync(
            Guid? tenantId,
            Guid userId,
            Guid friendId,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 添加黑名单
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        Task AddShieldMemberAsync(
            Guid? tenantId,
            Guid userId,
            Guid friendId,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 移除黑名单
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        Task RemoveShieldMemberAsync(
            Guid? tenantId,
            Guid userId,
            Guid friendId,
            CancellationToken cancellationToken = default);
    }
}
