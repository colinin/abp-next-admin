using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.Notifications;

public interface INotificationDefinitionGroupRecordRepository : IBasicRepository<NotificationDefinitionGroupRecord, Guid>
{
    Task<NotificationDefinitionGroupRecord> FindByNameAsync(
        string name, 
        CancellationToken cancellationToken = default);

    Task<List<NotificationDefinitionGroupRecord>> GetListAsync(
        ISpecification<NotificationDefinitionGroupRecord> specification,
        CancellationToken cancellationToken = default);
}
