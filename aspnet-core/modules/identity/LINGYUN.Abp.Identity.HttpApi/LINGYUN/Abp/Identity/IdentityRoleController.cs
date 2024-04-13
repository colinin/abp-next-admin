using Asp.Versioning;
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
    [ControllerName("Role")]
    [Route("api/identity/roles")]
    public class IdentityRoleController : AbpControllerBase, IIdentityRoleAppService
    {
        protected IIdentityRoleAppService RoleAppService { get; }
        public IdentityRoleController(
            IIdentityRoleAppService roleAppService) 
        {
            RoleAppService = roleAppService;
        }

        #region OrganizationUnit

        [HttpGet]
        [Route("{id}/organization-units")]
        public async virtual Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnitsAsync(Guid id)
        {
            return await RoleAppService.GetOrganizationUnitsAsync(id);
        }

        [HttpPut]
        [Route("{id}/organization-units")]
        public async virtual Task SetOrganizationUnitsAsync(Guid id, IdentityRoleAddOrRemoveOrganizationUnitDto input)
        {
            await RoleAppService.SetOrganizationUnitsAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}/organization-units/{ouId}")]
        public async virtual Task RemoveOrganizationUnitsAsync(Guid id, Guid ouId)
        {
            await RoleAppService.RemoveOrganizationUnitsAsync(id, ouId);
        }

        #endregion

        #region Claim

        [HttpGet]
        [Route("{id}/claims")]
        public async virtual Task<ListResultDto<IdentityClaimDto>> GetClaimsAsync(Guid id)
        {
            return await RoleAppService.GetClaimsAsync(id);
        }

        [HttpPost]
        [Route("{id}/claims")]
        public async virtual Task AddClaimAsync(Guid id, IdentityRoleClaimCreateDto input)
        {
            await RoleAppService.AddClaimAsync(id, input);
        }

        [HttpPut]
        [Route("{id}/claims")]
        public async virtual Task UpdateClaimAsync(Guid id, IdentityRoleClaimUpdateDto input)
        {
            await RoleAppService.UpdateClaimAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}/claims")]
        public async virtual Task DeleteClaimAsync(Guid id, IdentityRoleClaimDeleteDto input)
        {
            await RoleAppService.DeleteClaimAsync(id, input);
        }

        #endregion
    }
}
