using LINGYUN.Abp.IM.Messages;
using LINGYUN.Abp.MessageService.Groups;
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

        Task UpdateUserMessageAsync(
            UserMessage userMessage,
            CancellationToken cancellationToken = default);

        Task InsertGroupMessageAsync(
            GroupMessage groupMessage,
            CancellationToken cancellationToken = default);

        Task UpdateGroupMessageAsync(
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
            MessageType? type = null,
            string filter = "",
            CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(
            long groupId,
            MessageType? type = null,
            string filter = "",
            CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(
            Guid sendUserId,
            Guid receiveUserId,
            MessageType? type = null,
            string filter = "",
            CancellationToken cancellationToken = default);

        Task<List<LastChatMessage>> GetLastMessagesAsync(
            Guid userId,
            MessageState? state = null,
            string sorting = nameof(LastChatMessage.SendTime),
            int maxResultCount = 10,
            CancellationToken cancellationToken = default);

        Task<List<UserMessage>> GetUserMessagesAsync(
            Guid sendUserId,
            Guid receiveUserId,
            MessageType? type = null,
            string filter = "",
            string sorting = nameof(UserMessage.MessageId),
            int skipCount = 0,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default);

        Task<long> GetGroupMessagesCountAsync(
            long groupId,
            MessageType? type = null,
            string filter = "",
            CancellationToken cancellationToken = default);

        Task<List<GroupMessage>> GetGroupMessagesAsync(
            long groupId,
            MessageType? type = null,
            string filter = "",
            string sorting = nameof(UserMessage.MessageId),
            int skipCount = 0,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default);

        Task<long> GetUserGroupMessagesCountAsync(
            Guid sendUserId,
            long groupId,
            MessageType? type = null,
            string filter = "",
            CancellationToken cancellationToken = default);

        Task<List<GroupMessage>> GetUserGroupMessagesAsync(
            Guid sendUserId,
            long groupId,
            MessageType? type = null,
            string filter = "",
            string sorting = nameof(UserMessage.MessageId),
            int skipCount = 0,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default);
    }
}
