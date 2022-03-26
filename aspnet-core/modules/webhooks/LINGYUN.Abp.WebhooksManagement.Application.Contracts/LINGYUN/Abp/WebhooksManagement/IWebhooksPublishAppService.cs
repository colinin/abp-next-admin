using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.WebhooksManagement;

public interface IWebhooksPublishAppService : IApplicationService
{
    Task PublishAsync(WebhooksPublishInput input);
}
