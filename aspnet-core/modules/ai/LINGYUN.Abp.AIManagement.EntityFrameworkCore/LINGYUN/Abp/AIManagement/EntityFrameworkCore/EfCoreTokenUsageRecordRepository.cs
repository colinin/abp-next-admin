using LINGYUN.Abp.AIManagement.Tokens;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.AIManagement.EntityFrameworkCore;
public class EfCoreTokenUsageRecordRepository : EfCoreRepository<IAIManagementDbContext, TokenUsageRecord, Guid>, ITokenUsageRecordRepository
{
    public EfCoreTokenUsageRecordRepository(
        IDbContextProvider<IAIManagementDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async virtual Task<TokenUsageRecord?> FindByMessageIdAsync(
        Guid conversationId,
        Guid? messageId,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.ConversationId == conversationId && x.MessageId == messageId)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<TokenUsageRecord> specification,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<TokenUsageRecord>> GetListAsync(
        ISpecification<TokenUsageRecord> specification,
        string sorting = $"{nameof(TokenUsageRecord.CreationTime)}",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .OrderBy(!sorting.IsNullOrWhiteSpace() ? sorting : $"{nameof(TokenUsageRecord.CreationTime)}")
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
