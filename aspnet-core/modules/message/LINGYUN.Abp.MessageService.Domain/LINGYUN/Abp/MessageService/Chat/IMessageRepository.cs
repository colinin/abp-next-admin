using LINGYUN.Abp.IM.Messages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.MessageService.Chat
{
    public interface IMessageRepository
    {
        Task<UserMessage> InsertUserMessageAsync(UserMessage userMessage, bool saveChangs = false);

        Task<GroupMessage> InsertGroupMessageAsync(GroupMessage groupMessage, bool saveChangs = false);

        Task<UserMessage> GetUserMessageAsync(long id);

        Task<GroupMessage> GetGroupMessageAsync(long id);

        Task<long> GetUserMessagesCountAsync(Guid sendUserId, Guid receiveUserId, string filter = "", MessageType? type = null);

        Task<List<UserMessage>> GetUserMessagesAsync(Guid sendUserId, Guid receiveUserId, string filter = "", string sorting = nameof(UserMessage.MessageId), MessageType? type = null, int skipCount = 1, int maxResultCount = 10);

        Task<long> GetGroupMessagesCountAsync(long groupId, string filter = "", MessageType? type = null);

        Task<List<GroupMessage>> GetGroupMessagesAsync(long groupId, string filter = "", string sorting = nameof(UserMessage.MessageId), MessageType? type = null, int skipCount = 1, int maxResultCount = 10);

        Task<long> GetUserGroupMessagesCountAsync(Guid sendUserId, long groupId, string filter = "", MessageType? type = null);

        Task<List<GroupMessage>> GetUserGroupMessagesAsync(Guid sendUserId, long groupId, string filter = "", string sorting = nameof(UserMessage.MessageId), MessageType? type = null, int skipCount = 1, int maxResultCount = 10);
    }
}
