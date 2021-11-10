using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.MessageService.Groups
{
    public interface IGroupRepository : IBasicRepository<ChatGroup, long>
    {
        /// <summary>
        /// 用户是否已被拉黑
        /// </summary>
        /// <param name="id"></param>
        /// <param name="formUserId"></param>
        /// <returns></returns>
        Task<bool> UserHasBlackedAsync(
            long id,
            Guid formUserId,
            CancellationToken cancellationToken = default);

        Task<ChatGroup> FindByIdAsync(
            long id,
            CancellationToken cancellationToken = default);

        Task<List<UserGroupCard>> GetGroupAdminAsync(
            long id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取群组数
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> GetCountAsync(
            string filter = null,
            CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取群组列表
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sorting"></param>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<ChatGroup>> GetListAsync(
            string filter = null,
            string sorting = nameof(ChatGroup.Name),
            int skipCount = 0,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default);
    }
}
