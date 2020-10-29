using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.MessageService.Chat
{
    public interface IUserChatSettingRepository : IBasicRepository<UserChatSetting, long>
    {
        Task<bool> UserHasOpendImAsync(Guid userId);
        Task<UserChatSetting> FindByUserIdAsync(Guid userId);
    }
}
