using LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates;

[Authorize]
[Controller]
[Area("app")]
[RemoteService]
[Route("api/approval-flow/templates")]
public class TemplateController : AbpControllerBase, ITemplateAppService
{
    protected ITemplateAppService TemplateAppService { get; }

    public TemplateController(ITemplateAppService templateAppService)
    {
        TemplateAppService = templateAppService;
    }

    [HttpGet("/api/approval-flow/available-templates")]
    public virtual Task<ListResultDto<TemplateDto>> GetListAsync([FromQuery] bool onlyPublished = false)
    {
        return TemplateAppService.GetListAsync(onlyPublished);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<TemplateDto>> GetPagedListAsync([FromQuery] TemplateGetPagedListInput input)
    {
        return TemplateAppService.GetPagedListAsync(input);
    }

    [HttpGet("{id}")]
    public virtual Task<TemplateDto> GetAsync(Guid id)
    {
        return TemplateAppService.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<TemplateDto> CreateAsync([FromBody] TemplateCreateDto input)
    {
        return TemplateAppService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public virtual Task<TemplateDto> UpdateAsync(Guid id, [FromBody] TemplateUpdateDto input)
    {
        return TemplateAppService.UpdateAsync(id, input);
    }

    [HttpPost("{id}/publish")]
    public virtual Task<TemplateDto> PublishAsync(Guid id, [FromBody] TemplatePublishDto input)
    {
        return TemplateAppService.PublishAsync(id, input);
    }

    [HttpPut("{id}/design")]
    public virtual Task<TemplateDto> SaveDesignAsync(Guid id, [FromBody] TemplateSaveDesignDto input)
    {
        return TemplateAppService.SaveDesignAsync(id, input);
    }

    [HttpDelete("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return TemplateAppService.DeleteAsync(id);
    }
}
