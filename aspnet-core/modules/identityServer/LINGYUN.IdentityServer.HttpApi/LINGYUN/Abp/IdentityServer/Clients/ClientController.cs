using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    [RemoteService(Name = AbpIdentityServerConsts.RemoteServiceName)]
    [Area("IdentityServer")]
    [Route("api/IdentityServer/Clients")]
    public class ClientController : AbpController, IClientAppService
    {
        protected IClientAppService ClientAppService { get; }
        public ClientController(IClientAppService clientAppService)
        {
            ClientAppService = clientAppService;
        }

        [HttpPost]
        public virtual async Task<ClientDto> CreateAsync(ClientCreateDto clientCreate)
        {
            return await ClientAppService.CreateAsync(clientCreate);
        }

        [HttpDelete]
        [Route("{Id}")]
        public virtual async Task DeleteAsync(ClientGetByIdInputDto clientGetByIdInput)
        {
            await ClientAppService.DeleteAsync(clientGetByIdInput);
        }

        [HttpGet]
        [Route("{Id}")]
        public virtual async Task<ClientDto> GetAsync(ClientGetByIdInputDto clientGetById)
        {
            return await ClientAppService.GetAsync(clientGetById);
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<ClientDto>> GetAsync(ClientGetByPagedInputDto clientGetByPaged)
        {
            return await ClientAppService.GetAsync(clientGetByPaged);
        }

        [HttpPut]
        [Route("{Id}")]
        public virtual async Task<ClientDto> UpdateAsync(ClientUpdateDto clientUpdate)
        {
            return await ClientAppService.UpdateAsync(clientUpdate);
        }

        [HttpPost]
        [Route("Claims")]
        public virtual async Task<ClientClaimDto> AddClaimAsync(ClientClaimCreateDto clientClaimCreate)
        {
            return await ClientAppService.AddClaimAsync(clientClaimCreate);
        }

        [HttpPut]
        [Route("Claims")]
        public virtual async Task<ClientClaimDto> UpdateClaimAsync(ClientClaimUpdateDto clientClaimUpdate)
        {
            return await ClientAppService.UpdateClaimAsync(clientClaimUpdate);
        }

        [HttpDelete]
        [Route("Claims")]
        public virtual async Task DeleteClaimAsync(ClientClaimGetByKeyInputDto clientClaimGetByKey)
        {
            await ClientAppService.DeleteClaimAsync(clientClaimGetByKey);
        }

        [HttpPost]
        [Route("Properties")]
        public virtual async Task<ClientPropertyDto> AddPropertyAsync(ClientPropertyCreateDto clientPropertyCreate)
        {
            return await ClientAppService.AddPropertyAsync(clientPropertyCreate);
        }

        [HttpPut]
        [Route("Properties")]
        public virtual async Task<ClientPropertyDto> UpdatePropertyAsync(ClientPropertyUpdateDto clientPropertyUpdate)
        {
            return await ClientAppService.UpdatePropertyAsync(clientPropertyUpdate);
        }

        [HttpDelete]
        [Route("Properties")]
        public virtual async Task DeletePropertyAsync(ClientPropertyGetByKeyDto clientPropertyGetByKey)
        {
            await ClientAppService.DeletePropertyAsync(clientPropertyGetByKey);
        }

        [HttpPost]
        [Route("Secrets")]
        public virtual async Task<ClientSecretDto> AddSecretAsync(ClientSecretCreateDto clientSecretCreate)
        {
            return await ClientAppService.AddSecretAsync(clientSecretCreate);
        }

        [HttpDelete]
        [Route("Secrets")]
        public virtual async Task DeleteSecretAsync(ClientSecretGetByTypeDto clientSecretGetByType)
        {
            await ClientAppService.DeleteSecretAsync(clientSecretGetByType);
        }

        [HttpPut]
        [Route("Secrets")]
        public virtual async Task<ClientSecretDto> UpdateSecretAsync(ClientSecretUpdateDto clientSecretUpdate)
        {
            return await ClientAppService.UpdateSecretAsync(clientSecretUpdate);
        }
    }
}
