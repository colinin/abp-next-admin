using LINGYUN.Abp.AIManagement.Messages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.AIManagement.EntityFrameworkCore;
public class EfCoreUserTextMessageRecordRepository : EfCoreRepository<IAIManagementDbContext, UserTextMessageRecord, Guid>, IUserTextMessageRecordRepository
{
    public EfCoreUserTextMessageRecordRepository(
        IDbContextProvider<IAIManagementDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async virtual Task<IEnumerable<UserTextMessageRecord>> GetHistoryMessagesAsync(
        string conversationId,
        int maxResultCount = 0, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.ConversationId == conversationId)
            .OrderByDescending(x => x.CreationTime)
            .Take(maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
