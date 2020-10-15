using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Identity
{
    public interface IIdentityUserAppService : IApplicationService
    {

        #region OrganizationUnit

        Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnitsAsync(Guid id);

        Task UpdateOrganizationUnitsAsync(Guid id, IdentityUserOrganizationUnitUpdateDto input);

        #endregion

        #region ClaimType

        Task<ListResultDto<IdentityClaimDto>> GetClaimsAsync(Guid id);

        Task AddClaimAsync(Guid id, IdentityUserClaimCreateDto input);

        Task UpdateClaimAsync(Guid id, IdentityUserClaimUpdateDto input);

        Task DeleteClaimAsync(Guid id, IdentityUserClaimDeleteDto input);

        #endregion

    }
}
