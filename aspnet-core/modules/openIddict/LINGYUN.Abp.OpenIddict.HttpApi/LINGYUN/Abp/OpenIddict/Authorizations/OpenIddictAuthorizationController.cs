using LINGYUN.Abp.OpenIddict.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.OpenIddict.Authorizations;

[Controller]
[Authorize(AbpOpenIddictPermissions.Authorizations.Default)]
[RemoteService(Name = OpenIddictRemoteServiceConsts.RemoteServiceName)]
[Area(OpenIddictRemoteServiceConsts.ModuleName)]
[Route("api/openiddict/authorizations")]
public class OpenIddictAuthorizationController : OpenIddictControllerBase, IOpenIddictAuthorizationAppService
{
    protected IOpenIddictAuthorizationAppService Service { get; }

    public OpenIddictAuthorizationController(IOpenIddictAuthorizationAppService service)
    {
        Service = service;
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(AbpOpenIddictPermissions.Authorizations.Delete)]
    public virtual Task DeleteAsync(Guid id)
    {
        return Service.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<OpenIddictAuthorizationDto> GetAsync(Guid id)
    {
        return Service.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<OpenIddictAuthorizationDto>> GetListAsync(OpenIddictAuthorizationGetListInput input)
    {
        return Service.GetListAsync(input);
    }
}
