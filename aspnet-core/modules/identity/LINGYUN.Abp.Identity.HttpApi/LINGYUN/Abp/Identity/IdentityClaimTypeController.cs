using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity
{
    [RemoteService(true, Name = IdentityRemoteServiceConsts.RemoteServiceName)]
    [Area("identity")]
    [ControllerName("ClaimType")]
    [Route("api/identity/claim-types")]
    public class IdentityClaimTypeController : AbpController, IIdentityClaimTypeAppService
    {
        protected IIdentityClaimTypeAppService IdentityClaimTypeAppService { get; }
        public IdentityClaimTypeController(IIdentityClaimTypeAppService identityClaimTypeAppService)
        {
            IdentityClaimTypeAppService = identityClaimTypeAppService;
        }

        [HttpPost]
        public virtual async Task<IdentityClaimTypeDto> CreateAsync(IdentityClaimTypeCreateDto input)
        {
            return await IdentityClaimTypeAppService.CreateAsync(input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await IdentityClaimTypeAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("actived-list")]
        public virtual async Task<ListResultDto<IdentityClaimTypeDto>> GetAllListAsync()
        {
            return await IdentityClaimTypeAppService.GetAllListAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public virtual async Task<IdentityClaimTypeDto> GetAsync(Guid id)
        {
            return await IdentityClaimTypeAppService.GetAsync(id);
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<IdentityClaimTypeDto>> GetListAsync(IdentityClaimTypeGetByPagedDto input)
        {
            return await IdentityClaimTypeAppService.GetListAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual async Task<IdentityClaimTypeDto> UpdateAsync(Guid id, IdentityClaimTypeUpdateDto input)
        {
            return await IdentityClaimTypeAppService.UpdateAsync(id, input);
        }
    }
}
