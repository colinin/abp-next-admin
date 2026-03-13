using LINGYUN.Abp.AIManagement.Chats;
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
public class EfCoreTextChatMessageRecordRepository : EfCoreRepository<IAIManagementDbContext, TextChatMessageRecord, Guid>, ITextChatMessageRecordRepository
{
    public EfCoreTextChatMessageRecordRepository(
        IDbContextProvider<IAIManagementDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async virtual Task<IEnumerable<TextChatMessageRecord>> GetHistoryMessagesAsync(
        Guid conversationId,
        int maxResultCount = 0, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.ConversationId == conversationId)
            .OrderByDescending(x => x.CreationTime)
            .Take(maxResultCount)
            .OrderBy(x => x.CreationTime)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<TextChatMessageRecord> specification,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<TextChatMessageRecord>> GetListAsync(
        ISpecification<TextChatMessageRecord> specification,
        string? sorting = $"{nameof(TextChatMessageRecord.CreatedAt)}",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .OrderBy(!sorting.IsNullOrWhiteSpace() ? sorting : $"{nameof(TextChatMessageRecord.CreatedAt)}")
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
