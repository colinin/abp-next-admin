using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;

namespace LINGYUN.Abp.Identity
{
    [Authorize(IdentityPermissions.OrganizationUnits.Default)]
    public class OrganizationUnitAppService : IdentityAppServiceBase, IOrganizationUnitAppService
    {
        protected OrganizationUnitManager OrganizationUnitManager { get; }
        protected IOrganizationUnitRepository OrganizationUnitRepository { get; }

        protected IdentityUserManager UserManager { get; }
        protected IIdentityUserRepository UserRepository { get; }

        public OrganizationUnitAppService(
            IdentityUserManager userManager,
            IIdentityUserRepository userRepository,
            OrganizationUnitManager organizationUnitManager,
            IOrganizationUnitRepository organizationUnitRepository)
        {
            UserManager = userManager;
            UserRepository = userRepository;
            OrganizationUnitManager = organizationUnitManager;
            OrganizationUnitRepository = organizationUnitRepository;

            ObjectMapperContext = typeof(AbpIdentityApplicationModule);
        }

        [Authorize(IdentityPermissions.OrganizationUnits.Create)]
        public virtual async Task<OrganizationUnitDto> CreateAsync(OrganizationUnitCreateDto input)
        {
            var origanizationUnit = new OrganizationUnit(
                GuidGenerator.Create(), input.DisplayName, input.ParentId, CurrentTenant.Id)
            {
                CreationTime = Clock.Now
            };
            input.MapExtraPropertiesTo(origanizationUnit);

            await OrganizationUnitManager.CreateAsync(origanizationUnit);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<OrganizationUnit, OrganizationUnitDto>(origanizationUnit);
        }

        [Authorize(IdentityPermissions.OrganizationUnits.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var origanizationUnit = await OrganizationUnitRepository.FindAsync(id);
            if (origanizationUnit == null)
            {
                return;
            }
            await OrganizationUnitManager.DeleteAsync(id);
        }

        public virtual async Task<ListResultDto<OrganizationUnitDto>> FindChildrenAsync(OrganizationUnitGetChildrenDto input)
        {
            var origanizationUnitChildren = await OrganizationUnitManager.FindChildrenAsync(input.Id, input.Recursive);

            return new ListResultDto<OrganizationUnitDto>(
                ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(origanizationUnitChildren));
        }

        public virtual async Task<OrganizationUnitDto> GetAsync(Guid id)
        {
            var origanizationUnit = await OrganizationUnitRepository.FindAsync(id);

            return ObjectMapper.Map<OrganizationUnit, OrganizationUnitDto>(origanizationUnit);
        }

        public virtual async Task<OrganizationUnitDto> GetLastChildOrNullAsync(Guid? parentId)
        {
            var origanizationUnitLastChildren = await OrganizationUnitManager.GetLastChildOrNullAsync(parentId);

            return ObjectMapper.Map<OrganizationUnit, OrganizationUnitDto>(origanizationUnitLastChildren);
        }

        public virtual async Task<PagedResultDto<OrganizationUnitDto>> GetListAsync(OrganizationUnitGetByPagedDto input)
        {
            var origanizationUnitCount = await OrganizationUnitRepository.GetCountAsync();
            var origanizationUnits = await OrganizationUnitRepository
                .GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, false);

            return new PagedResultDto<OrganizationUnitDto>(origanizationUnitCount,
                ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(origanizationUnits));
        }

        [Authorize(IdentityPermissions.OrganizationUnits.ManageRoles)]
        public virtual async Task<PagedResultDto<IdentityRoleDto>> GetRolesAsync(OrganizationUnitGetRoleByPagedDto input)
        {
            var origanizationUnit = await OrganizationUnitRepository.GetAsync(input.Id);
            var origanizationUnitRoleCount = await OrganizationUnitRepository.GetRolesCountAsync(origanizationUnit);
            var origanizationUnitRoles = await OrganizationUnitRepository.GetRolesAsync(origanizationUnit, 
                input.Sorting, input.MaxResultCount, input.SkipCount, false);

            return new PagedResultDto<IdentityRoleDto>(origanizationUnitRoleCount,
                ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(origanizationUnitRoles));
        }

        [Authorize(IdentityPermissions.OrganizationUnits.ManageUsers)]
        public virtual async Task<ListResultDto<IdentityUserDto>> GetUsersAsync(OrganizationUnitGetUserDto input)
        {
            var origanizationUnit = await OrganizationUnitRepository.GetAsync(input.Id);
            // TODO: 官方库没有定义分页查询API，有可能是企业付费版本，需要自行实现
            var origanizationUnitUsers = await UserRepository.GetUsersInOrganizationUnitAsync(origanizationUnit.Id);

            return new ListResultDto<IdentityUserDto>(
                ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(origanizationUnitUsers));
        }

        [Authorize(IdentityPermissions.OrganizationUnits.Update)]
        public virtual async Task MoveAsync(Guid id, OrganizationUnitMoveDto input)
        {
            await OrganizationUnitManager.MoveAsync(id, input.ParentId);
        }

        [Authorize(IdentityPermissions.OrganizationUnits.Update)]
        public virtual async Task<OrganizationUnitDto> UpdateAsync(Guid id, OrganizationUnitUpdateDto input)
        {
            var origanizationUnit = await OrganizationUnitRepository.GetAsync(id);
            origanizationUnit.DisplayName = input.DisplayName;
            input.MapExtraPropertiesTo(origanizationUnit);

            await OrganizationUnitManager.UpdateAsync(origanizationUnit);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<OrganizationUnit, OrganizationUnitDto>(origanizationUnit);
        }

        [Authorize(IdentityPermissions.OrganizationUnits.ManageRoles)]
        public virtual async Task AddRoleAsync(OrganizationUnitDtoAddOrRemoveRoleDto input)
        {
            var origanizationUnit = await OrganizationUnitRepository.GetAsync(input.Id);
            if (!origanizationUnit.IsInRole(input.RoleId))
            {
                origanizationUnit.AddRole(input.RoleId);
                await OrganizationUnitManager.UpdateAsync(origanizationUnit);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }

        [Authorize(IdentityPermissions.OrganizationUnits.ManageRoles)]
        public virtual async Task RemoveRoleAsync(OrganizationUnitDtoAddOrRemoveRoleDto input)
        {
            var origanizationUnit = await OrganizationUnitRepository.GetAsync(input.Id);
            if (origanizationUnit.IsInRole(input.RoleId))
            {
                origanizationUnit.RemoveRole(input.RoleId);
                await OrganizationUnitManager.UpdateAsync(origanizationUnit);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }

        [Authorize(IdentityPermissions.OrganizationUnits.ManageUsers)]
        public virtual async Task AddUserAsync(OrganizationUnitDtoAddOrRemoveUserDto input)
        {
            var identityUser = await UserRepository.GetAsync(input.UserId);
            var origanizationUnit = await OrganizationUnitRepository.GetAsync(input.Id);
            if (!identityUser.IsInOrganizationUnit(input.Id))
            {
                await UserManager.AddToOrganizationUnitAsync(identityUser, origanizationUnit);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }

        [Authorize(IdentityPermissions.OrganizationUnits.ManageUsers)]
        public virtual async Task RemoveUserAsync(OrganizationUnitDtoAddOrRemoveUserDto input)
        {
            var identityUser = await UserRepository.GetAsync(input.UserId);
            var origanizationUnit = await OrganizationUnitRepository.GetAsync(input.Id);
            if (identityUser.IsInOrganizationUnit(input.Id))
            {
                await UserManager.RemoveFromOrganizationUnitAsync(identityUser, origanizationUnit);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }
    }
}
