using System;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.WebhooksManagement;

public interface IWebhookEventRecordRepository : IRepository<WebhookEventRecord, Guid>
{
}
