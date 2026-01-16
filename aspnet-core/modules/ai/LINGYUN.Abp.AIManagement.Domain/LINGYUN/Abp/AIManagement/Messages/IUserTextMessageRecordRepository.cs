using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.AIManagement.Messages;
public interface IUserTextMessageRecordRepository : IBasicRepository<UserTextMessageRecord, Guid>
{
    Task<IEnumerable<UserTextMessageRecord>> GetHistoryMessagesAsync(
        string conversationId,
        int maxResultCount = 0,
        CancellationToken cancellationToken = default);
}
