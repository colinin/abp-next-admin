using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.IdentityServer.Grants;

[RemoteService(Name = AbpIdentityServerConsts.RemoteServiceName)]
[Area("identity-server")]
[Route("api/identity-server/persisted-grants")]
public class PersistedGrantController : AbpControllerBase, IPersistedGrantAppService
{
    protected IPersistedGrantAppService PersistedGrantAppService { get; }

    public PersistedGrantController(
        IPersistedGrantAppService persistedGrantAppService)
    {
        PersistedGrantAppService = persistedGrantAppService;
    }

    [HttpDelete]
    [Route("{id}")]
    public async virtual Task DeleteAsync(Guid id)
    {
        await PersistedGrantAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id}")]
    public async virtual Task<PersistedGrantDto> GetAsync(Guid id)
    {
        return await PersistedGrantAppService.GetAsync(id);
    }

    [HttpGet]
    public async virtual Task<PagedResultDto<PersistedGrantDto>> GetListAsync(GetPersistedGrantInput input)
    {
        return await PersistedGrantAppService.GetListAsync(input);
    }
}
