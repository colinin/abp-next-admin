using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.Notifications;

public interface INotificationDefinitionRecordRepository : IBasicRepository<NotificationDefinitionRecord, Guid>
{
    Task<NotificationDefinitionRecord> FindByNameAsync(
        string name, 
        CancellationToken cancellationToken = default);

    Task<List<NotificationDefinitionRecord>> GetListAsync(
        ISpecification<NotificationDefinitionRecord> specification,
        CancellationToken cancellationToken = default);
}
