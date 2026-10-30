using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.OrganizationUnitManagers;

public interface IOrganizationUnitManagerRepository : IRepository<OrganizationUnitManagerAssignment, Guid>
{
	Task<List<OrganizationUnitManagerAssignment>> GetListByOrganizationUnitAsync(Guid organizationUnitId, CancellationToken cancellationToken = default);

	Task<List<OrganizationUnitManagerAssignment>> GetListByUserAsync(Guid userId, CancellationToken cancellationToken = default);

	Task<OrganizationUnitManagerAssignment?> FindByUserAsync(Guid organizationUnitId, Guid userId, CancellationToken cancellationToken = default);

	Task<OrganizationUnitManagerAssignment?> FindPrimaryAsync(Guid organizationUnitId, CancellationToken cancellationToken = default);

	Task<List<OrganizationUnitManagerAssignment>> GetPrimaryListAsync(IReadOnlyCollection<Guid> organizationUnitIds, CancellationToken cancellationToken = default);

	Task<List<OrganizationUnitManagerAssignment>> GetPagedListAsync(Guid? organizationUnitId, Guid? userId, string? sorting, int skipCount, int maxResultCount, CancellationToken cancellationToken = default);

	Task<long> GetCountAsync(Guid? organizationUnitId, Guid? userId, CancellationToken cancellationToken = default);
}
