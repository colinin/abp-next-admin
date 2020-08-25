using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Identity
{
    public interface IIdentityRoleAppService : IApplicationService
    {
        Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnitsAsync(Guid id);
        Task SetOrganizationUnitsAsync(Guid id, IdentityRoleAddOrRemoveOrganizationUnitDto input);
    }
}
