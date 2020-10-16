using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    [RemoteService(Name = AbpIdentityServerConsts.RemoteServiceName)]
    [Area("identity-server")]
    [Route("api/identity-server/clients")]
    public class ClientController : AbpController, IClientAppService
    {
        protected IClientAppService ClientAppService { get; }
        public ClientController(IClientAppService clientAppService)
        {
            ClientAppService = clientAppService;
        }

        [HttpPost]
        public virtual async Task<ClientDto> CreateAsync(ClientCreateDto input)
        {
            return await ClientAppService.CreateAsync(input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await ClientAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual async Task<ClientDto> GetAsync(Guid id)
        {
            return await ClientAppService.GetAsync(id);
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<ClientDto>> GetListAsync(ClientGetByPagedDto input)
        {
            return await ClientAppService.GetListAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual async Task<ClientDto> UpdateAsync(Guid id, ClientUpdateDto input)
        {
            return await ClientAppService.UpdateAsync(id, input);
        }

        [HttpPost]
        [Route("{id}/clone")]
        public virtual async Task<ClientDto> CloneAsync(Guid id, ClientCloneDto input)
        {
            return await ClientAppService.CloneAsync(id, input);
        }
    }
}
