using LINGYUN.Abp.WebhooksManagement.Authorization;
using LINGYUN.Abp.WebhooksManagement.Definitions.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.WebhooksManagement.Definitions;

[RemoteService(Name = WebhooksManagementRemoteServiceConsts.RemoteServiceName)]
[Area(WebhooksManagementRemoteServiceConsts.ModuleName)]
[Authorize(WebhooksManagementPermissions.WebhookDefinition.Default)]
[Route("api/webhooks/definitions")]
public class WebhookDefinitionController : WebhooksManagementControllerBase, IWebhookDefinitionAppService
{
    private readonly IWebhookDefinitionAppService _service;

    public WebhookDefinitionController(IWebhookDefinitionAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(WebhooksManagementPermissions.WebhookDefinition.Create)]
    public virtual Task<WebhookDefinitionDto> CreateAsync(WebhookDefinitionCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpDelete]
    [Route("{name}")]
    [Authorize(WebhooksManagementPermissions.WebhookDefinition.Delete)]
    public virtual Task DeleteAsync(string name)
    {
        return _service.DeleteAsync(name);
    }

    [HttpGet]
    [Route("{name}")]
    public virtual Task<WebhookDefinitionDto> GetAsync(string name)
    {
        return _service.GetAsync(name);
    }

    [HttpGet]
    public virtual Task<ListResultDto<WebhookDefinitionDto>> GetListAsync(WebhookDefinitionGetListInput input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPut]
    [Route("{name}")]
    [Authorize(WebhooksManagementPermissions.WebhookDefinition.Update)]
    public virtual Task<WebhookDefinitionDto> UpdateAsync(string name, WebhookDefinitionUpdateDto input)
    {
        return _service.UpdateAsync(name, input);
    }
}
