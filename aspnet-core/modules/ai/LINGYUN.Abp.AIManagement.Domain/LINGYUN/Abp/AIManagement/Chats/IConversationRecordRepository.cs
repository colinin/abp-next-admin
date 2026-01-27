using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.AIManagement.Chats;
public interface IConversationRecordRepository : IBasicRepository<ConversationRecord, Guid>
{
    Task<int> GetCountAsync(
        ISpecification<ConversationRecord> specification,
        CancellationToken cancellationToken = default);

    Task<List<ConversationRecord>> GetListAsync(
        ISpecification<ConversationRecord> specification,
        string? sorting = $"{nameof(ConversationRecord.CreatedAt)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
