using LINGYUN.Abp.PrintDesigner.Permissions;
using LINGYUN.Abp.PrintDesigner.Templates.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.PrintDesigner.Templates;

[Authorize(PrintDesignerPermissions.Template.Default)]
[Area(PrintDesignerRemoteServiceConsts.RemoteServiceName)]
[RemoteService(Name = PrintDesignerRemoteServiceConsts.RemoteServiceName)]
[Route($"api/{PrintDesignerRemoteServiceConsts.ModuleName}/templates")]
public class TemplateController(ITemplateAppService _service) : AbpControllerBase, ITemplateAppService
{
    [HttpPost]
    [Authorize(PrintDesignerPermissions.Template.Create)]
    public virtual Task<TemplateDto> CreateAsync(TemplateCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpDelete("{id}")]
    [Authorize(PrintDesignerPermissions.Template.Delete)]
    public virtual Task<TemplateDeleteDto> DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpGet("{id}")]
    public virtual Task<TemplateDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet("by-name/{name}")]
    public virtual Task<TemplateDto> GetByNameAsync(string name)
    {
        return _service.GetByNameAsync(name);
    }

    [HttpGet("all")]
    public virtual Task<ListResultDto<TemplateDto>> GetAllAsync()
    {
        return _service.GetAllAsync();
    }

    [HttpGet]
    public virtual Task<PagedResultDto<TemplateDto>> GetListAsync(TemplateGetPagedListInput input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPut("{id}")]
    [Authorize(PrintDesignerPermissions.Template.Update)]
    public virtual Task<TemplateDto> UpdateAsync(Guid id, TemplateUpdateDto input)
    {
        return _service.UpdateAsync(id, input);
    }

    [HttpPost("upsert")]
    public virtual Task<TemplateDto> UpsertAsync(TemplateUpsertDto input)
    {
        return _service.UpsertAsync(input);
    }
}
