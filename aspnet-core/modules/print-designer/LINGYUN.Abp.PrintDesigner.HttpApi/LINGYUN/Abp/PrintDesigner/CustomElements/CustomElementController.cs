using LINGYUN.Abp.PrintDesigner.CustomElements.Dtos;
using LINGYUN.Abp.PrintDesigner.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.PrintDesigner.CustomElements;

[Authorize(PrintDesignerPermissions.CustomElement.Default)]
[Area(PrintDesignerRemoteServiceConsts.RemoteServiceName)]
[RemoteService(Name = PrintDesignerRemoteServiceConsts.RemoteServiceName)]
[Route($"api/{PrintDesignerRemoteServiceConsts.ModuleName}/custom-elements")]
public class CustomElementController(ICustomElementAppService _service) : AbpControllerBase, ICustomElementAppService
{
    [HttpPost]
    [Authorize(PrintDesignerPermissions.CustomElement.Create)]
    public virtual Task<CustomElementDto> CreateAsync(CustomElementCreateDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpDelete("{id}")]
    [Authorize(PrintDesignerPermissions.CustomElement.Delete)]
    public virtual Task<CustomElementDeleteDto> DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpGet("{id}")]
    public virtual Task<CustomElementDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet("all")]
    public virtual Task<ListResultDto<CustomElementDto>> GetAllAsync()
    {
        return _service.GetAllAsync();
    }

    [HttpGet]
    public virtual Task<PagedResultDto<CustomElementDto>> GetListAsync(CustomElementGetPagedListInput input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPut("{id}")]
    [Authorize(PrintDesignerPermissions.CustomElement.Update)]
    public virtual Task<CustomElementDto> UpdateAsync(Guid id, CustomElementUpdateDto input)
    {
        return _service.UpdateAsync(id, input);
    }

    [HttpPost("upsert")]
    public virtual Task<CustomElementDto> UpsertAsync(CustomElementUpsertDto input)
    {
        return _service.UpsertAsync(input);
    }
}
