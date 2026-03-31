using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.AIManagement.Tokens;
public interface ITokenUsageRecordRepository : IBasicRepository<TokenUsageRecord, Guid>
{
    Task<TokenUsageRecord?> FindByMessageIdAsync(
        Guid conversationId,
        Guid? messageId,
        CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(
        ISpecification<TokenUsageRecord> specification,
        CancellationToken cancellationToken = default);

    Task<List<TokenUsageRecord>> GetListAsync(
        ISpecification<TokenUsageRecord> specification,
        string sorting = $"{nameof(TokenUsageRecord.CreationTime)}",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
