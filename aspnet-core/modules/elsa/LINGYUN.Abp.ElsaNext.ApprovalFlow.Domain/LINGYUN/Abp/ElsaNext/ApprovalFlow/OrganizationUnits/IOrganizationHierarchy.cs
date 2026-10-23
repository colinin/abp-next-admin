using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.OrganizationUnits;

public interface IOrganizationHierarchy
{
	Task<IReadOnlyList<OrganizationUnitSupervisor>> GetSupervisorChainAsync(Guid organizationUnitId, int maxLevels, CancellationToken cancellationToken = default);

	Task<Guid?> FindPrimaryOrganizationUnitIdAsync(Guid userId, CancellationToken cancellationToken = default);

	Task<IReadOnlyList<Guid>> GetMemberUserIdsAsync(Guid organizationUnitId, int skipCount, int maxResultCount, string? filter, CancellationToken cancellationToken = default);
}
