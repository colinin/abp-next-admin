using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity
{
    public interface IOrganizationUnitAppService : 
        ICrudAppService<OrganizationUnitDto,
                        Guid,
                        OrganizationUnitGetByPagedDto,
                        OrganizationUnitCreateDto,
                        OrganizationUnitUpdateDto>
    {
        Task<ListResultDto<OrganizationUnitDto>> GetAllListAsync();

        Task<OrganizationUnitDto> GetLastChildOrNullAsync(Guid? parentId);

        Task MoveAsync(Guid id, OrganizationUnitMoveDto input);

        Task<ListResultDto<OrganizationUnitDto>> GetRootAsync();

        Task<ListResultDto<OrganizationUnitDto>> FindChildrenAsync(OrganizationUnitGetChildrenDto input);

        Task<ListResultDto<string>> GetRoleNamesAsync(Guid id);

        Task<PagedResultDto<IdentityRoleDto>> GetUnaddedRolesAsync(Guid id, OrganizationUnitGetUnaddedRoleByPagedDto input);

        Task<PagedResultDto<IdentityRoleDto>> GetRolesAsync(Guid id, PagedAndSortedResultRequestDto input);

        Task AddRolesAsync(Guid id, OrganizationUnitAddRoleDto input);

        Task<PagedResultDto<IdentityUserDto>> GetUnaddedUsersAsync(Guid id, OrganizationUnitGetUnaddedUserByPagedDto input);

        Task<PagedResultDto<IdentityUserDto>> GetUsersAsync(Guid id, GetIdentityUsersInput input);

        Task AddUsersAsync(Guid id, OrganizationUnitAddUserDto input);
    }
}
