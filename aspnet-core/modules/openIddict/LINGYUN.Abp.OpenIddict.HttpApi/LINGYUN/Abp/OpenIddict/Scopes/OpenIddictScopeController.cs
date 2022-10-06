using LINGYUN.Abp.OpenIddict.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.OpenIddict.Scopes;

[Controller]
[Authorize(AbpOpenIddictPermissions.Scopes.Default)]
[RemoteService(Name = OpenIddictRemoteServiceConsts.RemoteServiceName)]
[Area(OpenIddictRemoteServiceConsts.ModuleName)]
[Route("api/openiddict/scopes")]
public class OpenIddictScopeController : OpenIddictControllerBase, IOpenIddictScopeAppService
{
    protected IOpenIddictScopeAppService Service { get; }

    public OpenIddictScopeController(IOpenIddictScopeAppService service)
    {
        Service = service;
    }

    [HttpPost]
    [Authorize(AbpOpenIddictPermissions.Scopes.Create)]
    public virtual Task<OpenIddictScopeDto> CreateAsync(OpenIddictScopeCreateDto input)
    {
        return Service.CreateAsync(input);
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(AbpOpenIddictPermissions.Scopes.Delete)]
    public virtual Task DeleteAsync(Guid id)
    {
        return Service.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<OpenIddictScopeDto> GetAsync(Guid id)
    {
        return Service.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<OpenIddictScopeDto>> GetListAsync(OpenIddictScopeGetListInput input)
    {
        return Service.GetListAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(AbpOpenIddictPermissions.Scopes.Update)]
    public virtual Task<OpenIddictScopeDto> UpdateAsync(Guid id, OpenIddictScopeUpdateDto input)
    {
        return Service.UpdateAsync(id, input);
    }
}
