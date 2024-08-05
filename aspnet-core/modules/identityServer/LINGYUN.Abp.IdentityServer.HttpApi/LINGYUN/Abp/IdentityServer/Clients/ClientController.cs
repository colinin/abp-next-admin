using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.IdentityServer.Clients;

[RemoteService(Name = AbpIdentityServerConsts.RemoteServiceName)]
[Area("identity-server")]
[Route("api/identity-server/clients")]
public class ClientController : AbpControllerBase, IClientAppService
{
    protected IClientAppService ClientAppService { get; }
    public ClientController(IClientAppService clientAppService)
    {
        ClientAppService = clientAppService;
    }

    [HttpPost]
    public async virtual Task<ClientDto> CreateAsync(ClientCreateDto input)
    {
        return await ClientAppService.CreateAsync(input);
    }

    [HttpDelete]
    [Route("{id}")]
    public async virtual Task DeleteAsync(Guid id)
    {
        await ClientAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("{id}")]
    public async virtual Task<ClientDto> GetAsync(Guid id)
    {
        return await ClientAppService.GetAsync(id);
    }

    [HttpGet]
    public async virtual Task<PagedResultDto<ClientDto>> GetListAsync(ClientGetByPagedDto input)
    {
        return await ClientAppService.GetListAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public async virtual Task<ClientDto> UpdateAsync(Guid id, ClientUpdateDto input)
    {
        return await ClientAppService.UpdateAsync(id, input);
    }

    [HttpPost]
    [Route("{id}/clone")]
    public async virtual Task<ClientDto> CloneAsync(Guid id, ClientCloneDto input)
    {
        return await ClientAppService.CloneAsync(id, input);
    }

    [HttpGet]
    [Route("assignable-api-resources")]
    public async virtual Task<ListResultDto<string>> GetAssignableApiResourcesAsync()
    {
        return await ClientAppService.GetAssignableApiResourcesAsync();
    }

    [HttpGet]
    [Route("assignable-identity-resources")]
    public async virtual Task<ListResultDto<string>> GetAssignableIdentityResourcesAsync()
    {
        return await ClientAppService.GetAssignableIdentityResourcesAsync();
    }

    [HttpGet]
    [Route("distinct-cors-origins")]
    public async virtual Task<ListResultDto<string>> GetAllDistinctAllowedCorsOriginsAsync()
    {
        return await ClientAppService.GetAllDistinctAllowedCorsOriginsAsync();
    }
}
