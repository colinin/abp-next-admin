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
    [ControllerName("User")]
    [Route("api/identity/users")]
    public class IdentityUserController : AbpController, IIdentityUserAppService
    {
        protected IIdentityUserAppService UserAppService { get; }
        public IdentityUserController(
            IIdentityUserAppService userAppService)
        {
            UserAppService = userAppService;
        }

        #region OrganizationUnit

        [HttpGet]
        [Route("organization-units/{id}")]
        public virtual async Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnitsAsync(Guid id)
        {
            return await UserAppService.GetOrganizationUnitsAsync(id);
        }

        [HttpPut]
        [Route("organization-units/{id}")]
        public virtual async Task UpdateOrganizationUnitsAsync(Guid id, IdentityUserOrganizationUnitUpdateDto input)
        {
            await UserAppService.UpdateOrganizationUnitsAsync(id, input);
        }

        #endregion

        #region Claim

        [HttpGet]
        [Route("claims/{id}")]
        public virtual async Task<ListResultDto<IdentityClaimDto>> GetClaimsAsync(Guid id)
        {
            return await UserAppService.GetClaimsAsync(id);
        }

        [HttpPost]
        [Route("claims/{id}")]
        public virtual async Task AddClaimAsync(Guid id, IdentityUserClaimCreateDto input)
        {
            await UserAppService.AddClaimAsync(id, input);
        }

        [HttpPut]
        [Route("claims/{id}")]
        public virtual async Task UpdateClaimAsync(Guid id, IdentityUserClaimUpdateDto input)
        {
            await UserAppService.UpdateClaimAsync(id, input);
        }

        [HttpDelete]
        [Route("claims/{id}")]
        public virtual async Task DeleteClaimAsync(Guid id, IdentityUserClaimDeleteDto input)
        {
            await UserAppService.DeleteClaimAsync(id, input);
        }

        #endregion
    }
}
