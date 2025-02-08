using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Platform.Messages;
public interface IEmailMessageRepository : IBasicRepository<EmailMessage, Guid>
{
    Task<int> GetCountAsync(
        ISpecification<EmailMessage> specification,
        CancellationToken cancellationToken = default);

    Task<List<EmailMessage>> GetListAsync(
        ISpecification<EmailMessage> specification,
        string sorting = $"{nameof(Message.CreationTime)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
