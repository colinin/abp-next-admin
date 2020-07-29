using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity
{
    [RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
    [Area("identity")]
    [ControllerName("organization-unit")]
    [Route("api/identity/organization-unit")]
    public class IOrganizationUnitController : AbpController, IOrganizationUnitAppService
    {
        protected IOrganizationUnitAppService OrganizationUnitAppService { get; }

        public IOrganizationUnitController(
            IOrganizationUnitAppService organizationUnitAppService)
        {
            OrganizationUnitAppService = organizationUnitAppService;
        }

        [HttpPost]
        [Route("management-roles")]
        public virtual async Task AddRoleAsync(OrganizationUnitDtoAddOrRemoveRoleDto input)
        {
            await OrganizationUnitAppService.AddRoleAsync(input);
        }

        [HttpPost]
        [Route("management-users")]
        public virtual async Task AddUserAsync(OrganizationUnitDtoAddOrRemoveUserDto input)
        {
            await OrganizationUnitAppService.AddUserAsync(input);
        }

        [HttpPost]
        public virtual async Task<OrganizationUnitDto> CreateAsync(OrganizationUnitCreateDto input)
        {
            return await OrganizationUnitAppService.CreateAsync(input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await OrganizationUnitAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("find-children")]
        public virtual async Task<ListResultDto<OrganizationUnitDto>> FindChildrenAsync(OrganizationUnitGetChildrenDto input)
        {
            return await OrganizationUnitAppService.FindChildrenAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual async Task<OrganizationUnitDto> GetAsync(Guid id)
        {
            return await OrganizationUnitAppService.GetAsync(id);
        }

        [HttpGet]
        [Route("last-children")]
        public virtual async Task<OrganizationUnitDto> GetLastChildOrNullAsync([Required] Guid parentId)
        {
            return await OrganizationUnitAppService.GetLastChildOrNullAsync(parentId);
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<OrganizationUnitDto>> GetListAsync(OrganizationUnitGetByPagedDto input)
        {
            return await OrganizationUnitAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("management-roles")]
        public virtual async Task<PagedResultDto<IdentityRoleDto>> GetRolesAsync(OrganizationUnitGetRoleByPagedDto input)
        {
            return await OrganizationUnitAppService.GetRolesAsync(input);
        }

        [HttpGet]
        [Route("management-users")]
        public virtual async Task<ListResultDto<IdentityUserDto>> GetUsersAsync(OrganizationUnitGetUserDto input)
        {
            return await OrganizationUnitAppService.GetUsersAsync(input);
        }

        [HttpPut]
        [Route("{id}/move")]
        public virtual async Task MoveAsync(OrganizationUnitMoveDto input)
        {
            await OrganizationUnitAppService.MoveAsync(input);
        }

        [HttpDelete]
        [Route("management-roles")]
        public virtual async Task RemoveRoleAsync(OrganizationUnitDtoAddOrRemoveRoleDto input)
        {
            await OrganizationUnitAppService.RemoveRoleAsync(input);
        }

        [HttpDelete]
        [Route("management-users")]
        public virtual async Task RemoveUserAsync(OrganizationUnitDtoAddOrRemoveUserDto input)
        {
            await OrganizationUnitAppService.RemoveUserAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual async Task<OrganizationUnitDto> UpdateAsync(Guid id, OrganizationUnitUpdateDto input)
        {
            return await OrganizationUnitAppService.UpdateAsync(id, input);
        }
    }
}
