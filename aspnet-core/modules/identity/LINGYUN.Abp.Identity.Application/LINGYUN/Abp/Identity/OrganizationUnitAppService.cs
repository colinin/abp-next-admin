using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
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
        protected IIdentityRoleRepository RoleRepository { get; }
        protected IIdentityUserRepository UserRepository { get; }

        public OrganizationUnitAppService(
            IdentityUserManager userManager,
            IIdentityRoleRepository roleRepository,
            IIdentityUserRepository userRepository,
            OrganizationUnitManager organizationUnitManager,
            IOrganizationUnitRepository organizationUnitRepository)
        {
            UserManager = userManager;
            RoleRepository = roleRepository;
            UserRepository = userRepository;
            OrganizationUnitManager = organizationUnitManager;
            OrganizationUnitRepository = organizationUnitRepository;

            ObjectMapperContext = typeof(AbpIdentityApplicationModule);
        }

        [Authorize(IdentityPermissions.OrganizationUnits.Create)]
        public virtual async Task<OrganizationUnitDto> CreateAsync(OrganizationUnitCreateDto input)
        {
            var organizationUnit = new OrganizationUnit(
                GuidGenerator.Create(), input.DisplayName, input.ParentId, CurrentTenant.Id)
            {
                CreationTime = Clock.Now
            };
            input.MapExtraPropertiesTo(organizationUnit);

            await OrganizationUnitManager.CreateAsync(organizationUnit);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<OrganizationUnit, OrganizationUnitDto>(organizationUnit);
        }

        [Authorize(IdentityPermissions.OrganizationUnits.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var organizationUnit = await OrganizationUnitRepository.FindAsync(id);
            if (organizationUnit == null)
            {
                return;
            }
            await OrganizationUnitManager.DeleteAsync(id);
        }

        public virtual async Task<ListResultDto<OrganizationUnitDto>> GetRootAsync()
        {
            var rootOrganizationUnits = await OrganizationUnitManager.FindChildrenAsync(null, recursive: false);

            return new ListResultDto<OrganizationUnitDto>(
                ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(rootOrganizationUnits));
        }

        public virtual async Task<ListResultDto<OrganizationUnitDto>> FindChildrenAsync(OrganizationUnitGetChildrenDto input)
        {
            var organizationUnitChildren = await OrganizationUnitManager.FindChildrenAsync(input.Id, input.Recursive);

            return new ListResultDto<OrganizationUnitDto>(
                ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(organizationUnitChildren));
        }

        public virtual async Task<OrganizationUnitDto> GetAsync(Guid id)
        {
            var organizationUnit = await OrganizationUnitRepository.FindAsync(id);

            return ObjectMapper.Map<OrganizationUnit, OrganizationUnitDto>(organizationUnit);
        }

        public virtual async Task<OrganizationUnitDto> GetLastChildOrNullAsync(Guid? parentId)
        {
            var organizationUnitLastChildren = await OrganizationUnitManager.GetLastChildOrNullAsync(parentId);

            return ObjectMapper.Map<OrganizationUnit, OrganizationUnitDto>(organizationUnitLastChildren);
        }

        public virtual async Task<ListResultDto<OrganizationUnitDto>> GetAllListAsync()
        {
            var organizationUnits = await OrganizationUnitRepository.GetListAsync(false);

            return new ListResultDto<OrganizationUnitDto>(
                ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(organizationUnits));
        }

        public virtual async Task<PagedResultDto<OrganizationUnitDto>> GetListAsync(OrganizationUnitGetByPagedDto input)
        {
            var organizationUnitCount = await OrganizationUnitRepository.GetCountAsync();
            var organizationUnits = await OrganizationUnitRepository
                .GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, false);

            return new PagedResultDto<OrganizationUnitDto>(organizationUnitCount,
                ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(organizationUnits));
        }

        [Authorize(IdentityPermissions.OrganizationUnits.ManageRoles)]
        public virtual async Task<ListResultDto<string>> GetRoleNamesAsync(Guid id)
        {
            var inOrganizationUnitRoleNames = await UserRepository.GetRoleNamesInOrganizationUnitAsync(id);
            return new ListResultDto<string>(inOrganizationUnitRoleNames);
        }

        [Authorize(IdentityPermissions.OrganizationUnits.ManageRoles)]
        public virtual async Task<PagedResultDto<IdentityRoleDto>> GetUnaddedRolesAsync(Guid id, OrganizationUnitGetUnaddedRoleByPagedDto input)
        {
            var organizationUnit = await OrganizationUnitRepository.GetAsync(id);

            var organizationUnitRoleCount = await OrganizationUnitRepository
                .GetUnaddedRolesCountAsync(organizationUnit, input.Filter);

            var organizationUnitRoles = await OrganizationUnitRepository
                .GetUnaddedRolesAsync(organizationUnit, 
                input.Sorting, input.MaxResultCount, 
                input.SkipCount, input.Filter);

            return new PagedResultDto<IdentityRoleDto>(organizationUnitRoleCount,
                ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(organizationUnitRoles));
        }

        [Authorize(IdentityPermissions.OrganizationUnits.ManageRoles)]
        public virtual async Task<PagedResultDto<IdentityRoleDto>> GetRolesAsync(Guid id, PagedAndSortedResultRequestDto input)
        {
            var organizationUnit = await OrganizationUnitRepository.GetAsync(id);

            var organizationUnitRoleCount = await OrganizationUnitRepository
                .GetRolesCountAsync(organizationUnit);

            var organizationUnitRoles = await OrganizationUnitRepository
                .GetRolesAsync(organizationUnit,
                input.Sorting, input.MaxResultCount,
                input.SkipCount);

            return new PagedResultDto<IdentityRoleDto>(organizationUnitRoleCount,
                ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(organizationUnitRoles));
        }


        [Authorize(IdentityPermissions.OrganizationUnits.ManageUsers)]
        public virtual async Task<PagedResultDto<IdentityUserDto>> GetUnaddedUsersAsync(Guid id, OrganizationUnitGetUnaddedUserByPagedDto input)
        {
            var organizationUnit = await OrganizationUnitRepository.GetAsync(id);

            var organizationUnitUserCount = await OrganizationUnitRepository
                .GetUnaddedUsersCountAsync(organizationUnit, input.Filter);
            var organizationUnitUsers = await OrganizationUnitRepository
                .GetUnaddedUsersAsync(organizationUnit,
                input.Sorting, input.MaxResultCount, 
                input.SkipCount, input.Filter);

            return new PagedResultDto<IdentityUserDto>(organizationUnitUserCount,
                ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(organizationUnitUsers));
        }

        [Authorize(IdentityPermissions.OrganizationUnits.ManageUsers)]
        public virtual async Task<PagedResultDto<IdentityUserDto>> GetUsersAsync(Guid id, GetIdentityUsersInput input)
        {
            var organizationUnit = await OrganizationUnitRepository.GetAsync(id);

            var organizationUnitUserCount = await OrganizationUnitRepository
                .GetMembersCountAsync(organizationUnit, input.Filter);
            var organizationUnitUsers = await OrganizationUnitRepository
                .GetMembersAsync(organizationUnit,
                input.Sorting, input.MaxResultCount,
                input.SkipCount, input.Filter);

            return new PagedResultDto<IdentityUserDto>(organizationUnitUserCount,
                ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(organizationUnitUsers));
        }

        [Authorize(IdentityPermissions.OrganizationUnits.Update)]
        public virtual async Task MoveAsync(Guid id, OrganizationUnitMoveDto input)
        {
            await OrganizationUnitManager.MoveAsync(id, input.ParentId);
        }

        [Authorize(IdentityPermissions.OrganizationUnits.Update)]
        public virtual async Task<OrganizationUnitDto> UpdateAsync(Guid id, OrganizationUnitUpdateDto input)
        {
            var organizationUnit = await OrganizationUnitRepository.GetAsync(id);
            organizationUnit.DisplayName = input.DisplayName;
            input.MapExtraPropertiesTo(organizationUnit);

            await OrganizationUnitManager.UpdateAsync(organizationUnit);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<OrganizationUnit, OrganizationUnitDto>(organizationUnit);
        }

        [Authorize(IdentityPermissions.OrganizationUnits.ManageUsers)]
        public virtual async Task AddUsersAsync(Guid id, OrganizationUnitAddUserDto input)
        {
            var organizationUnit = await OrganizationUnitRepository.GetAsync(id);
            var users = await UserRepository.GetListByIdListAsync(input.UserIds, includeDetails: true);

            // 调用内部方法设置用户组织机构
            foreach (var user in users)
            {
                await UserManager.AddToOrganizationUnitAsync(user, organizationUnit);
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [Authorize(IdentityPermissions.OrganizationUnits.ManageRoles)]
        public virtual async Task AddRolesAsync(Guid id, OrganizationUnitAddRoleDto input)
        {
            var organizationUnit = await OrganizationUnitRepository.GetAsync(id);

            var roles = await RoleRepository.GetListByIdListAsync(input.RoleIds, includeDetails: true);

            foreach (var role in roles)
            {
                await OrganizationUnitManager.AddRoleToOrganizationUnitAsync(role, organizationUnit);
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }
}
