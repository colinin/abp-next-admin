using LINGYUN.Abp.IM.Messages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.MessageService.Messages
{
    public interface IMessageRepository
    {
        Task<UserMessage> InsertUserMessageAsync(UserMessage userMessage, bool saveChangs = false);

        Task<GroupMessage> InsertGroupMessageAsync(GroupMessage groupMessage, bool saveChangs = false);

        Task<UserMessage> GetUserMessageAsync(long id);

        Task<GroupMessage> GetGroupMessageAsync(long id);

        Task<List<UserMessage>> GetUserMessagesAsync(Guid sendUserId, Guid receiveUserId, string filter = "", MessageType type = MessageType.Text, int skipCount = 1, int maxResultCount = 10);

        Task<List<GroupMessage>> GetGroupMessagesAsync(long groupId, string filter = "", MessageType type = MessageType.Text, int skipCount = 1, int maxResultCount = 10);

        Task<List<GroupMessage>> GetUserGroupMessagesAsync(Guid sendUserId, long groupId, string filter = "", MessageType type = MessageType.Text, int skipCount = 1, int maxResultCount = 10);
    }
}
