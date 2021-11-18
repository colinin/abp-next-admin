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

        public virtual async Task<ListResultDto<OrganizationUnitDto>> GetRootAsync()
        {
            var rootOriganizationUnits = await OrganizationUnitManager.FindChildrenAsync(null, recursive: false);

            return new ListResultDto<OrganizationUnitDto>(
                ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(rootOriganizationUnits));
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

        public virtual async Task<ListResultDto<OrganizationUnitDto>> GetAllListAsync()
        {
            var origanizationUnits = await OrganizationUnitRepository.GetListAsync(false);

            return new ListResultDto<OrganizationUnitDto>(
                ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(origanizationUnits));
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
        public virtual async Task<ListResultDto<string>> GetRoleNamesAsync(Guid id)
        {
            var inOrignizationUnitRoleNames = await UserRepository.GetRoleNamesInOrganizationUnitAsync(id);
            return new ListResultDto<string>(inOrignizationUnitRoleNames);
        }

        [Authorize(IdentityPermissions.OrganizationUnits.ManageRoles)]
        public virtual async Task<PagedResultDto<IdentityRoleDto>> GetUnaddedRolesAsync(Guid id, OrganizationUnitGetUnaddedRoleByPagedDto input)
        {
            var origanizationUnit = await OrganizationUnitRepository.GetAsync(id);

            var origanizationUnitRoleCount = await OrganizationUnitRepository
                .GetUnaddedRolesCountAsync(origanizationUnit, input.Filter);

            var origanizationUnitRoles = await OrganizationUnitRepository
                .GetUnaddedRolesAsync(origanizationUnit, 
                input.Sorting, input.MaxResultCount, 
                input.SkipCount, input.Filter);

            return new PagedResultDto<IdentityRoleDto>(origanizationUnitRoleCount,
                ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(origanizationUnitRoles));
        }

        [Authorize(IdentityPermissions.OrganizationUnits.ManageRoles)]
        public virtual async Task<PagedResultDto<IdentityRoleDto>> GetRolesAsync(Guid id, PagedAndSortedResultRequestDto input)
        {
            var origanizationUnit = await OrganizationUnitRepository.GetAsync(id);

            var origanizationUnitRoleCount = await OrganizationUnitRepository
                .GetRolesCountAsync(origanizationUnit);

            var origanizationUnitRoles = await OrganizationUnitRepository
                .GetRolesAsync(origanizationUnit,
                input.Sorting, input.MaxResultCount,
                input.SkipCount);

            return new PagedResultDto<IdentityRoleDto>(origanizationUnitRoleCount,
                ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(origanizationUnitRoles));
        }


        [Authorize(IdentityPermissions.OrganizationUnits.ManageUsers)]
        public virtual async Task<PagedResultDto<IdentityUserDto>> GetUnaddedUsersAsync(Guid id, OrganizationUnitGetUnaddedUserByPagedDto input)
        {
            var origanizationUnit = await OrganizationUnitRepository.GetAsync(id);

            var origanizationUnitUserCount = await OrganizationUnitRepository
                .GetUnaddedUsersCountAsync(origanizationUnit, input.Filter);
            var origanizationUnitUsers = await OrganizationUnitRepository
                .GetUnaddedUsersAsync(origanizationUnit,
                input.Sorting, input.MaxResultCount, 
                input.SkipCount, input.Filter);

            return new PagedResultDto<IdentityUserDto>(origanizationUnitUserCount,
                ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(origanizationUnitUsers));
        }

        [Authorize(IdentityPermissions.OrganizationUnits.ManageUsers)]
        public virtual async Task<PagedResultDto<IdentityUserDto>> GetUsersAsync(Guid id, GetIdentityUsersInput input)
        {
            var origanizationUnit = await OrganizationUnitRepository.GetAsync(id);

            var origanizationUnitUserCount = await OrganizationUnitRepository
                .GetMembersCountAsync(origanizationUnit, input.Filter);
            var origanizationUnitUsers = await OrganizationUnitRepository
                .GetMembersAsync(origanizationUnit,
                input.Sorting, input.MaxResultCount,
                input.SkipCount, input.Filter);

            return new PagedResultDto<IdentityUserDto>(origanizationUnitUserCount,
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

        [Authorize(IdentityPermissions.OrganizationUnits.ManageUsers)]
        public virtual async Task AddUsersAsync(Guid id, OrganizationUnitAddUserDto input)
        {
            var origanizationUnit = await OrganizationUnitRepository.GetAsync(id);
            var users = await UserRepository.GetListByIdListAsync(input.UserIds, includeDetails: true);

            // 调用内部方法设置用户组织机构
            foreach (var user in users)
            {
                await UserManager.AddToOrganizationUnitAsync(user, origanizationUnit);
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [Authorize(IdentityPermissions.OrganizationUnits.ManageRoles)]
        public virtual async Task AddRolesAsync(Guid id, OrganizationUnitAddRoleDto input)
        {
            var origanizationUnit = await OrganizationUnitRepository.GetAsync(id);

            var roles = await RoleRepository.GetListByIdListAsync(input.RoleIds, includeDetails: true);

            foreach (var role in roles)
            {
                await OrganizationUnitManager.AddRoleToOrganizationUnitAsync(role, origanizationUnit);
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }
}
