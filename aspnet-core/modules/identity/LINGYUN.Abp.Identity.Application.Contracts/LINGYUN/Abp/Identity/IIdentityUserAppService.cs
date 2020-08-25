using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Identity
{
    public interface IIdentityUserAppService : IApplicationService
    {
        Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnitsAsync(Guid id);
        Task UpdateOrganizationUnitsAsync(Guid id, IdentityUserOrganizationUnitUpdateDto input);
    }
}
