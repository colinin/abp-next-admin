using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.IdentityServer.Grants
{
    [RemoteService(Name = AbpIdentityServerConsts.RemoteServiceName)]
    [Area("identity-server")]
    [Route("api/identity-server/persisted-grants")]
    public class PersistedGrantController : AbpController, IPersistedGrantAppService
    {
        protected IPersistedGrantAppService PersistedGrantAppService { get; }

        public PersistedGrantController(
            IPersistedGrantAppService persistedGrantAppService)
        {
            PersistedGrantAppService = persistedGrantAppService;
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await PersistedGrantAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual async Task<PersistedGrantDto> GetAsync(Guid id)
        {
            return await PersistedGrantAppService.GetAsync(id);
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<PersistedGrantDto>> GetListAsync(GetPersistedGrantInput input)
        {
            return await PersistedGrantAppService.GetListAsync(input);
        }
    }
}
