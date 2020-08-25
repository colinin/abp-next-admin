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
    public class IIdentityRoleController : AbpController, IIdentityRoleAppService
    {
        protected IIdentityRoleAppService RoleAppService { get; }
        public IIdentityRoleController(
            IIdentityRoleAppService roleAppService) 
        {
            RoleAppService = roleAppService;
        }

        [HttpGet]
        [Route("organization-units/{id}")]
        public virtual async Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnitsAsync(Guid id)
        {
            return await RoleAppService.GetOrganizationUnitsAsync(id);
        }

        [HttpPut]
        [Route("organization-units/{id}")]
        public virtual async Task SetOrganizationUnitsAsync(Guid id, IdentityRoleAddOrRemoveOrganizationUnitDto input)
        {
            await RoleAppService.SetOrganizationUnitsAsync(id, input);
        }
    }
}
