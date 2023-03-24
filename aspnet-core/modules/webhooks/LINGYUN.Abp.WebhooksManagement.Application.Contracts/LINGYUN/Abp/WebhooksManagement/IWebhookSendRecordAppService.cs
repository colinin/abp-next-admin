using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.WebhooksManagement;

public interface IWebhookSendRecordAppService : IApplicationService
{
    Task<WebhookSendRecordDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task DeleteManyAsync(WebhookSendRecordDeleteManyInput input);

    Task ResendAsync(Guid id);

    Task ResendManyAsync(WebhookSendRecordResendManyInput input);

    Task<PagedResultDto<WebhookSendRecordDto>> GetListAsync(WebhookSendRecordGetListInput input);
}
