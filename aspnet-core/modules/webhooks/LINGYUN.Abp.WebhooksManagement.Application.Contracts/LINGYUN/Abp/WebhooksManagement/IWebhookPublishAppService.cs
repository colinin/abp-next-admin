using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.WebhooksManagement;

public interface IWebhookPublishAppService : IApplicationService
{
    Task PublishAsync(WebhookPublishInput input);
}
