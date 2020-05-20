using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.IdentityServer.IdentityResources
{
    [RemoteService(Name = AbpIdentityServerConsts.RemoteServiceName)]
    [Area("IdentityServer")]
    [Route("api/IdentityServer/IdentityResources")]
    public class IdentityResourceController : AbpController, IIdentityResourceAppService
    {
        protected IIdentityResourceAppService IdentityResourceAppService { get; }
        public IdentityResourceController(
            IIdentityResourceAppService identityResourceAppService)
        {
            IdentityResourceAppService = identityResourceAppService;
        }

        [HttpGet]
        [Route("{Id}")]
        public virtual async Task<IdentityResourceDto> GetAsync(IdentityResourceGetByIdInputDto identityResourceGetById)
        {
            return await IdentityResourceAppService.GetAsync(identityResourceGetById);
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<IdentityResourceDto>> GetAsync(IdentityResourceGetByPagedInputDto identityResourceGetByPaged)
        {
            return await IdentityResourceAppService.GetAsync(identityResourceGetByPaged);
        }

        [HttpPost]
        public virtual async Task<IdentityResourceDto> CreateAsync(IdentityResourceCreateDto identityResourceCreate)
        {
            return await IdentityResourceAppService.CreateAsync(identityResourceCreate);
        }

        [HttpPut]
        public virtual async Task<IdentityResourceDto> UpdateAsync(IdentityResourceUpdateDto identityResourceUpdate)
        {
            return await IdentityResourceAppService.UpdateAsync(identityResourceUpdate);
        }

        [HttpDelete]
        public virtual async Task DeleteAsync(IdentityResourceGetByIdInputDto identityResourceGetById)
        {
            await IdentityResourceAppService.DeleteAsync(identityResourceGetById);
        }

        [HttpPost]
        [Route("Properties")]
        public virtual async Task<IdentityResourcePropertyDto> AddPropertyAsync(IdentityResourcePropertyCreateDto identityResourcePropertyCreate)
        {
            return await IdentityResourceAppService.AddPropertyAsync(identityResourcePropertyCreate);
        }

        [HttpDelete]
        [Route("Properties")]
        public virtual async Task DeletePropertyAsync(IdentityResourcePropertyGetByKeyDto identityResourcePropertyGetByKey)
        {
            await IdentityResourceAppService.DeletePropertyAsync(identityResourcePropertyGetByKey);
        }
    }
}
