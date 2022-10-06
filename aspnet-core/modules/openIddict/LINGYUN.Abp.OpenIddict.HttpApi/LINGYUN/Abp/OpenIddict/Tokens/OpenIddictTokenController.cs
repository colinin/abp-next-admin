using LINGYUN.Abp.OpenIddict.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.OpenIddict.Tokens;

[Controller]
[Authorize(AbpOpenIddictPermissions.Tokens.Default)]
[RemoteService(Name = OpenIddictRemoteServiceConsts.RemoteServiceName)]
[Area(OpenIddictRemoteServiceConsts.ModuleName)]
[Route("api/openiddict/tokens")]
public class OpenIddictTokenController : OpenIddictControllerBase, IOpenIddictTokenAppService
{
    protected IOpenIddictTokenAppService Service { get; }

    public OpenIddictTokenController(IOpenIddictTokenAppService service)
    {
        Service = service;
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(AbpOpenIddictPermissions.Tokens.Delete)]
    public virtual Task DeleteAsync(Guid id)
    {
        return Service.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<OpenIddictTokenDto> GetAsync(Guid id)
    {
        return Service.GetAsync(id);
    }

    [HttpGet]
    public virtual Task<PagedResultDto<OpenIddictTokenDto>> GetListAsync(OpenIddictTokenGetListInput input)
    {
        return Service.GetListAsync(input);
    }
}
