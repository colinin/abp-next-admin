using System;
using System.ComponentModel.DataAnnotations;
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
        Task<OrganizationUnitDto> GetLastChildOrNullAsync([Required] Guid parentId);

        Task MoveAsync(OrganizationUnitMoveDto input);

        Task<ListResultDto<OrganizationUnitDto>> FindChildrenAsync(OrganizationUnitGetChildrenDto input);

        Task<PagedResultDto<IdentityRoleDto>> GetRolesAsync(OrganizationUnitGetRoleByPagedDto input);

        Task<ListResultDto<IdentityUserDto>> GetUsersAsync(OrganizationUnitGetUserDto input);

        Task AddRoleAsync(OrganizationUnitDtoAddOrRemoveRoleDto input);

        Task RemoveRoleAsync(OrganizationUnitDtoAddOrRemoveRoleDto input);

        Task AddUserAsync(OrganizationUnitDtoAddOrRemoveUserDto input);

        Task RemoveUserAsync(OrganizationUnitDtoAddOrRemoveUserDto input);
    }
}
