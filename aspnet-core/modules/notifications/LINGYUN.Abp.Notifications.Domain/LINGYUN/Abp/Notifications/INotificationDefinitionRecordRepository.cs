using System;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.Notifications;

public interface INotificationDefinitionRecordRepository : IBasicRepository<NotificationDefinitionRecord, Guid>
{
}
