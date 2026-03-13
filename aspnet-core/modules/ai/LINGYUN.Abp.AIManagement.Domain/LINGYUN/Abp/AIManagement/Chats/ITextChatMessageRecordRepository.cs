using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.AIManagement.Chats;
public interface ITextChatMessageRecordRepository : IBasicRepository<TextChatMessageRecord, Guid>
{
    Task<IEnumerable<TextChatMessageRecord>> GetHistoryMessagesAsync(
        Guid conversationId,
        int maxResultCount = 0,
        CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(
        ISpecification<TextChatMessageRecord> specification,
        CancellationToken cancellationToken = default);

    Task<List<TextChatMessageRecord>> GetListAsync(
        ISpecification<TextChatMessageRecord> specification,
        string? sorting = $"{nameof(TextChatMessageRecord.CreatedAt)}",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
