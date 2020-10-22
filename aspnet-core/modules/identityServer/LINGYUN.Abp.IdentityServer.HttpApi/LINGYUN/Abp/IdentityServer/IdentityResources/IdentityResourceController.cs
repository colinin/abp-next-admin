using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.IdentityServer.IdentityResources
{
    [RemoteService(Name = AbpIdentityServerConsts.RemoteServiceName)]
    [Area("identity-server")]
    [Route("api/identity-server/identity-resources")]
    public class IdentityResourceController : AbpController, IIdentityResourceAppService
    {
        protected IIdentityResourceAppService IdentityResourceAppService { get; }
        public IdentityResourceController(
            IIdentityResourceAppService identityResourceAppService)
        {
            IdentityResourceAppService = identityResourceAppService;
        }

        [HttpGet]
        [Route("{id}")]
        public virtual async Task<IdentityResourceDto> GetAsync(Guid id)
        {
            return await IdentityResourceAppService.GetAsync(id);
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<IdentityResourceDto>> GetListAsync(IdentityResourceGetByPagedDto input)
        {
            return await IdentityResourceAppService.GetListAsync(input);
        }

        [HttpPost]
        public virtual async Task<IdentityResourceDto> CreateAsync(IdentityResourceCreateOrUpdateDto input)
        {
            return await IdentityResourceAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual async Task<IdentityResourceDto> UpdateAsync(Guid id, IdentityResourceCreateOrUpdateDto input)
        {
            return await IdentityResourceAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await IdentityResourceAppService.DeleteAsync(id);
        }
    }
}
