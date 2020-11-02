using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.MessageService.Group
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

    }
}
