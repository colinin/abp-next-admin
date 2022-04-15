using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity
{
    public interface IIdentityRoleRepository : Volo.Abp.Identity.IIdentityRoleRepository
    {
        Task<List<IdentityRole>> GetListByIdListAsync(
            List<Guid> roleIds,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
            );

        Task<List<OrganizationUnit>> GetOrganizationUnitsAsync(
            Guid id,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        Task<List<OrganizationUnit>> GetOrganizationUnitsAsync(
            IEnumerable<string> roleNames,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        Task<List<IdentityRole>> GetRolesInOrganizationUnitAsync(
            Guid organizationUnitId,
            CancellationToken cancellationToken = default
            );
        Task<List<IdentityRole>> GetRolesInOrganizationsListAsync(
            List<Guid> organizationUnitIds,
            CancellationToken cancellationToken = default
            );

        Task<List<IdentityRole>> GetRolesInOrganizationUnitWithChildrenAsync(
            string code,
            CancellationToken cancellationToken = default
            );
    }
}
