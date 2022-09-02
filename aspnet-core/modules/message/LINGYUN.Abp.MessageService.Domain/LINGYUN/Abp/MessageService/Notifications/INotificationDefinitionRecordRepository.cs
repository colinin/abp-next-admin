using System;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.MessageService.Notifications;

public interface INotificationDefinitionRecordRepository : IBasicRepository<NotificationDefinitionRecord, Guid>
{
}
