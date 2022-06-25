using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.MessageService.Notifications;

public interface INotificationTemplateRepository : IBasicRepository<NotificationTemplate, Guid>
{
    Task<NotificationTemplate> GetByNameAsync(string name, string culture = null, CancellationToken cancellationToken = default);
}
