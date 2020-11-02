using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.MessageService.Group;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.MessageService.Chat
{
    public interface IMessageRepository
    {
        Task InsertUserMessageAsync(
            UserMessage userMessage,
            CancellationToken cancellationToken = default);

        Task InsertGroupMessageAsync(
            GroupMessage groupMessage,
            CancellationToken cancellationToken = default);

        Task<UserMessage> GetUserMessageAsync(
            long id, 
            CancellationToken cancellationToken = default);

        Task<GroupMessage> GetGroupMessageAsync(
            long id, 
            CancellationToken cancellationToken = default);

        Task<long> GetUserMessagesCountAsync(
            Guid sendUserId, 
            Guid receiveUserId, 
            string filter = "", 
            MessageType? type = null, 
            CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(
            long groupId,
            string filter = "",
            MessageType? type = null,
            CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(
            Guid sendUserId,
            Guid receiveUserId,
            string filter = "",
            MessageType? type = null,
            CancellationToken cancellationToken = default);

        Task<List<LastChatMessage>> GetLastMessagesByOneFriendAsync(
            Guid userId,
            string sorting = nameof(LastChatMessage.SendTime),
            bool reverse = true,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default);

        Task<List<UserMessage>> GetUserMessagesAsync(
            Guid sendUserId,
            Guid receiveUserId,
            string filter = "", 
            string sorting = nameof(UserMessage.MessageId), 
            bool reverse = true,
            MessageType? type = null, 
            int skipCount = 0, 
            int maxResultCount = 10,
            CancellationToken cancellationToken = default);

        Task<long> GetGroupMessagesCountAsync(
            long groupId, 
            string filter = "",
            MessageType? type = null,
            CancellationToken cancellationToken = default);

        Task<List<GroupMessage>> GetGroupMessagesAsync(
            long groupId,
            string filter = "",
            string sorting = nameof(UserMessage.MessageId),
            bool reverse = true,
            MessageType? type = null, 
            int skipCount = 0, 
            int maxResultCount = 10,
            CancellationToken cancellationToken = default);

        Task<long> GetUserGroupMessagesCountAsync(
            Guid sendUserId, 
            long groupId, 
            string filter = "", 
            MessageType? type = null,
            CancellationToken cancellationToken = default);

        Task<List<GroupMessage>> GetUserGroupMessagesAsync(
            Guid sendUserId, 
            long groupId, 
            string filter = "",
            string sorting = nameof(UserMessage.MessageId),
            bool reverse = true,
            MessageType? type = null, 
            int skipCount = 0, 
            int maxResultCount = 10,
            CancellationToken cancellationToken = default);
    }
}
