using LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions;

[Authorize]
[Controller]
[Area("app")]
[RemoteService]
[Route("api/approval-flow/form-definitions")]
public class FormDefinitionController : AbpControllerBase, IFormDefinitionAppService
{
    protected IFormDefinitionAppService FormDefinitionAppService { get; }

    public FormDefinitionController(IFormDefinitionAppService formDefinitionAppService)
    {
        FormDefinitionAppService = formDefinitionAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<FormDefinitionDto>> GetPagedListAsync([FromQuery] FormDefinitionGetPagedListInput input)
    {
        return FormDefinitionAppService.GetPagedListAsync(input);
    }

    [HttpGet("{id}")]
    public virtual Task<FormDefinitionDto> GetAsync(Guid id)
    {
        return FormDefinitionAppService.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<FormDefinitionDto> CreateAsync([FromBody] FormDefinitionCreateDto input)
    {
        return FormDefinitionAppService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public virtual Task<FormDefinitionDto> UpdateAsync(Guid id, [FromBody] FormDefinitionUpdateDto input)
    {
        return FormDefinitionAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return FormDefinitionAppService.DeleteAsync(id);
    }

    [HttpPost("{id}/publish")]
    public virtual Task<FormDefinitionDto> PublishAsync(Guid id)
    {
        return FormDefinitionAppService.PublishAsync(id);
    }

    [HttpPost("{id}/fields")]
    public virtual Task<FormDefinitionDto> AddFieldAsync(Guid id, [FromBody] FormFieldCreateDto input)
    {
        return FormDefinitionAppService.AddFieldAsync(id, input);
    }

    [HttpPut("{id}/fields/{fieldId}")]
    public virtual Task<FormDefinitionDto> UpdateFieldAsync(Guid id, Guid fieldId, [FromBody] FormFieldUpdateDto input)
    {
        return FormDefinitionAppService.UpdateFieldAsync(id, fieldId, input);
    }

    [HttpDelete("{id}/fields/{fieldId}")]
    public virtual Task<FormDefinitionDto> RemoveFieldAsync(Guid id, Guid fieldId)
    {
        return FormDefinitionAppService.RemoveFieldAsync(id, fieldId);
    }

    [HttpPut("{id}/data/{instanceId}")]
    public virtual Task<FormDataDto> SaveDataAsync(Guid id, string instanceId, [FromBody] FormDataSaveDto input)
    {
        return FormDefinitionAppService.SaveDataAsync(id, instanceId, input);
    }

    [HttpGet("{id}/data/{instanceId}")]
    public virtual Task<FormDataDto?> GetDataAsync(Guid id, string instanceId)
    {
        return FormDefinitionAppService.GetDataAsync(id, instanceId);
    }
}
