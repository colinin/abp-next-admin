using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.MessageService.Messages
{
    public interface IGroupRepository : IBasicRepository<ChatGroup, long>
    {
        /// <summary>
        /// 用户是否已被拉黑
        /// </summary>
        /// <param name="id"></param>
        /// <param name="formUserId"></param>
        /// <returns></returns>
        Task<bool> UserHasBlackedAsync(long id, Guid formUserId);

        Task<ChatGroup> GetByIdAsync(long id);

        Task<List<ChatGroupAdmin>> GetGroupAdminAsync(long id);
    }
}
