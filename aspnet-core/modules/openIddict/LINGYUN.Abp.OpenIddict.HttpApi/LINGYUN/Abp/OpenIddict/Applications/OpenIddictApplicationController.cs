using LINGYUN.Abp.OpenIddict.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.OpenIddict.Applications;

[Controller]
[Authorize(AbpOpenIddictPermissions.Applications.Default)]
[RemoteService(Name = OpenIddictRemoteServiceConsts.RemoteServiceName)]
[Area(OpenIddictRemoteServiceConsts.ModuleName)]
[Route("api/openiddict/applications")]
public class OpenIddictApplicationController : OpenIddictControllerBase, IOpenIddictApplicationAppService
{
    protected IOpenIddictApplicationAppService Service { get; }

    public OpenIddictApplicationController(IOpenIddictApplicationAppService service)
    {
        Service = service;
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<OpenIddictApplicationDto> GetAsync(Guid id)
    {
        return Service.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<OpenIddictApplicationDto>> GetListAsync(OpenIddictApplicationGetListInput input)
    {
        return Service.GetListAsync(input);
    }

    [HttpPost]
    [Authorize(AbpOpenIddictPermissions.Applications.Create)]
    public virtual Task<OpenIddictApplicationDto> CreateAsync(OpenIddictApplicationCreateDto input)
    {
        return Service.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(AbpOpenIddictPermissions.Applications.Update)]
    public virtual Task<OpenIddictApplicationDto> UpdateAsync(Guid id, OpenIddictApplicationUpdateDto input)
    {
        return Service.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(AbpOpenIddictPermissions.Applications.Delete)]
    public virtual Task DeleteAsync(Guid id)
    {
        return Service.DeleteAsync(id);
    }
}
