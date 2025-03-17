using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.WebhooksManagement.Integration;

[IntegrationService]
public interface IWebhookPublishIntegrationService : IApplicationService
{
    Task PublishAsync(WebhookPublishInput input);
}
