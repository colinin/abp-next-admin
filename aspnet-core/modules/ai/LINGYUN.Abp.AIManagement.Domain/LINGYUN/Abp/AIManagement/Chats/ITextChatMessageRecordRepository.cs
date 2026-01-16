using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.AIManagement.Chats;
public interface ITextChatMessageRecordRepository : IBasicRepository<TextChatMessageRecord, Guid>
{
    Task<IEnumerable<TextChatMessageRecord>> GetHistoryMessagesAsync(
        string conversationId,
        int maxResultCount = 0,
        CancellationToken cancellationToken = default);
}
