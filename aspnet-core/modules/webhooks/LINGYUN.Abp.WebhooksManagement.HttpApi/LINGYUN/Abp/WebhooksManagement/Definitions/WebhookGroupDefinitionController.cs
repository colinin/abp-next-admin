using LINGYUN.Abp.WebhooksManagement.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.WebhooksManagement.Definitions;

[RemoteService(Name = WebhooksManagementRemoteServiceConsts.RemoteServiceName)]
[Area(WebhooksManagementRemoteServiceConsts.ModuleName)]
[Authorize(WebhooksManagementPermissions.WebhookGroupDefinition.Default)]
[Route("api/webhooks/definitions/groups")]
public class WebhookGroupDefinitionController : WebhooksManagementControllerBase, IWebhookGroupDefinitionAppService
{
    private readonly IWebhookGroupDefinitionAppService _service;

    public WebhookGroupDefinitionController(IWebhookGroupDefinitionAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(WebhooksManagementPermissions.WebhookGroupDefinition.Create)]
    public virtual Task<WebhookGroupDefinitionDto> CreateAsync(WebhookGroupDefinitionCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpDelete]
    [Route("{name}")]
    [Authorize(WebhooksManagementPermissions.WebhookGroupDefinition.Delete)]
    public virtual Task DeleteAysnc(string name)
    {
        return _service.DeleteAysnc(name);
    }

    [HttpGet]
    [Route("{name}")]
    public virtual Task<WebhookGroupDefinitionDto> GetAsync(string name)
    {
        return _service.GetAsync(name);
    }

    [HttpGet]
    public virtual Task<ListResultDto<WebhookGroupDefinitionDto>> GetListAsync(WebhookGroupDefinitionGetListInput input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPut]
    [Route("{name}")]
    [Authorize(WebhooksManagementPermissions.WebhookGroupDefinition.Update)]
    public virtual Task<WebhookGroupDefinitionDto> UpdateAsync(string name, WebhookGroupDefinitionUpdateDto input)
    {
        return _service.UpdateAsync(name, input);
    }
}
