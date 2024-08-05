using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.IdentityServer.IdentityResources;

[RemoteService(Name = AbpIdentityServerConsts.RemoteServiceName)]
[Area("identity-server")]
[Route("api/identity-server/identity-resources")]
public class IdentityResourceController : AbpControllerBase, IIdentityResourceAppService
{
    protected IIdentityResourceAppService IdentityResourceAppService { get; }
    public IdentityResourceController(
        IIdentityResourceAppService identityResourceAppService)
    {
        IdentityResourceAppService = identityResourceAppService;
    }

    [HttpGet]
    [Route("{id}")]
    public async virtual Task<IdentityResourceDto> GetAsync(Guid id)
    {
        return await IdentityResourceAppService.GetAsync(id);
    }

    [HttpGet]
    public async virtual Task<PagedResultDto<IdentityResourceDto>> GetListAsync(IdentityResourceGetByPagedDto input)
    {
        return await IdentityResourceAppService.GetListAsync(input);
    }

    [HttpPost]
    public async virtual Task<IdentityResourceDto> CreateAsync(IdentityResourceCreateOrUpdateDto input)
    {
        return await IdentityResourceAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public async virtual Task<IdentityResourceDto> UpdateAsync(Guid id, IdentityResourceCreateOrUpdateDto input)
    {
        return await IdentityResourceAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public async virtual Task DeleteAsync(Guid id)
    {
        await IdentityResourceAppService.DeleteAsync(id);
    }
}
