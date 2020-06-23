using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.MessageService.Chat
{
    public interface IUserChatSettingRepository : IBasicRepository<UserChatSetting, long>
    {
        Task<bool> UserHasOpendImAsync(Guid userId);
        /// <summary>
        /// 用户是否已被拉黑
        /// </summary>
        /// <param name="formUserId"></param>
        /// <param name="toUserId"></param>
        /// <returns></returns>
        Task<bool> UserHasBlackedAsync(Guid formUserId, Guid toUserId);

        Task<UserChatSetting> GetByUserIdAsync(Guid userId);
    }
}
