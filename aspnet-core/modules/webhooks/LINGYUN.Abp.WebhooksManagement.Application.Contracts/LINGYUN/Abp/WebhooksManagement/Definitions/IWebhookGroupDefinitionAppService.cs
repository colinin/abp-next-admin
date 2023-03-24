using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.WebhooksManagement.Definitions;

public interface IWebhookGroupDefinitionAppService : IApplicationService
{
    Task<WebhookGroupDefinitionDto> GetAsync(string name);

    Task DeleteAysnc(string name);

    Task<WebhookGroupDefinitionDto> CreateAsync(WebhookGroupDefinitionCreateDto input);

    Task<WebhookGroupDefinitionDto> UpdateAsync(string name, WebhookGroupDefinitionUpdateDto input);

    Task<PagedResultDto<WebhookGroupDefinitionDto>> GetListAsync(WebhookGroupDefinitionGetListInput input);
}
