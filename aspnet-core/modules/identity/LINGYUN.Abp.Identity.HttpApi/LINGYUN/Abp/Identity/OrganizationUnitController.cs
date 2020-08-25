using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity
{
    [RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
    [Area("identity")]
    [ControllerName("organization-units")]
    [Route("api/identity/organization-units")]
    public class OrganizationUnitController : AbpController, IOrganizationUnitAppService
    {
        protected IOrganizationUnitAppService OrganizationUnitAppService { get; }

        public OrganizationUnitController(
            IOrganizationUnitAppService organizationUnitAppService)
        {
            OrganizationUnitAppService = organizationUnitAppService;
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
        [Route("root-node")]
        public virtual async Task<ListResultDto<OrganizationUnitDto>> GetRootAsync()
        {
            return await OrganizationUnitAppService.GetRootAsync();
        }

        [HttpGet]
        [Route("last-children")]
        public virtual async Task<OrganizationUnitDto> GetLastChildOrNullAsync(Guid? parentId)
        {
            return await OrganizationUnitAppService.GetLastChildOrNullAsync(parentId);
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<OrganizationUnitDto>> GetListAsync(OrganizationUnitGetByPagedDto input)
        {
            return await OrganizationUnitAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("management-roles/{id}")]
        public virtual async Task<ListResultDto<string>> GetRoleNamesAsync(Guid id)
        {
            return await OrganizationUnitAppService.GetRoleNamesAsync(id);
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
        public virtual async Task MoveAsync(Guid id, OrganizationUnitMoveDto input)
        {
            await OrganizationUnitAppService.MoveAsync(id, input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual async Task<OrganizationUnitDto> UpdateAsync(Guid id, OrganizationUnitUpdateDto input)
        {
            return await OrganizationUnitAppService.UpdateAsync(id, input);
        }
    }
}
