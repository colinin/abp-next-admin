using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Platform.Messages;
public interface ISmsMessageRepository : IBasicRepository<SmsMessage, Guid>
{
    Task<int> GetCountAsync(
        ISpecification<SmsMessage> specification,
        CancellationToken cancellationToken = default);

    Task<List<SmsMessage>> GetListAsync(
        ISpecification<SmsMessage> specification,
        string sorting = $"{nameof(Message.CreationTime)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
