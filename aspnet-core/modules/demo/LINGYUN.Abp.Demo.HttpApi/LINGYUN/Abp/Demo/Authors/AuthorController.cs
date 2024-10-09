using LINGYUN.Abp.Demo.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Demo.Authors;

[Controller]
[Authorize(DemoPermissions.Authors.Default)]
[RemoteService(Name = DemoRemoteServiceConsts.RemoteServiceName)]
[Area(DemoRemoteServiceConsts.ModuleName)]
[Route($"api/{DemoRemoteServiceConsts.ModuleName}/authors")]
public class AuthorController : AbpControllerBase, IAuthorAppService
{
    private readonly IAuthorAppService _service;
    public AuthorController(IAuthorAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(DemoPermissions.Authors.Create)]
    public virtual Task<AuthorDto> CreateAsync(CreateAuthorDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(DemoPermissions.Authors.Delete)]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<AuthorDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<AuthorDto>> GetListAsync(GetAuthorListDto input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(DemoPermissions.Authors.Edit)]
    public virtual Task UpdateAsync(Guid id, UpdateAuthorDto input)
    {
        return _service.UpdateAsync(id, input);
    }
}
