using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.WebhooksManagement;

public interface IWebhookSendRecordAppService : IApplicationService
{
    Task<WebhookSendRecordDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task ResendAsync(Guid id);

    Task<PagedResultDto<WebhookSendRecordDto>> GetListAsync(WebhookSendRecordGetListInput input);
}
