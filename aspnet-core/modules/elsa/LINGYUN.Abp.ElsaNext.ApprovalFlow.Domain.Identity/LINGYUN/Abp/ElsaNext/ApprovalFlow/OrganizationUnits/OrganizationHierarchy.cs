using LINGYUN.Abp.ElsaNext.ApprovalFlow.OrganizationUnitManagers;
using LINGYUN.Abp.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;
using IIdentityUserRepository = LINGYUN.Abp.Identity.IIdentityUserRepository;
using IOrganizationUnitRepository = LINGYUN.Abp.Identity.IOrganizationUnitRepository;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.OrganizationUnits;

public class OrganizationHierarchy : DomainService, IOrganizationHierarchy
{
	protected IOrganizationUnitRepository OrganizationUnitRepository { get; }

	protected IOrganizationUnitManagerRepository ManagerRepository { get; }

	protected IIdentityUserRepository IdentityUserRepository { get; }

	public OrganizationHierarchy(IOrganizationUnitRepository organizationUnitRepository, IOrganizationUnitManagerRepository managerRepository, IIdentityUserRepository identityUserRepository)
	{
		OrganizationUnitRepository = organizationUnitRepository;
		ManagerRepository = managerRepository;
		IdentityUserRepository = identityUserRepository;
	}

	public virtual async Task<IReadOnlyList<OrganizationUnitSupervisor>> GetSupervisorChainAsync(Guid organizationUnitId, int maxLevels, CancellationToken cancellationToken = default)
	{
		var supervisors = new List<OrganizationUnitSupervisor>();
		var limit = Math.Min((maxLevels <= 0) ? OrganizationUnitConsts.MaxSupervisorLevels : maxLevels, OrganizationUnitConsts.MaxSupervisorLevels);
		Guid? currentId = organizationUnitId;
		var depth = 0;
		while (currentId.HasValue && depth < OrganizationUnitConsts.MaxSupervisorLevels)
		{
			depth++;
			var organizationUnit = await OrganizationUnitRepository.FindAsync(currentId.Value, true, cancellationToken);
			if (organizationUnit == null)
			{
				break;
			}
			var managers = await ManagerRepository.GetListByOrganizationUnitAsync(organizationUnit.Id, cancellationToken);
			if (managers.Count > 0)
			{
				supervisors.Add(new OrganizationUnitSupervisor(
					ManagerUserIds: (from x in managers
						orderby x.IsPrimary descending, x.Order
						select x.UserId).ToArray(),
					OrganizationUnitId: organizationUnit.Id,
					OrganizationUnitDisplayName: organizationUnit.DisplayName,
					Level: supervisors.Count + 1));
				if (supervisors.Count >= limit)
				{
					break;
				}
			}
			currentId = organizationUnit.ParentId;
		}
		return supervisors;
	}

	public virtual async Task<Guid?> FindPrimaryOrganizationUnitIdAsync(Guid userId, CancellationToken cancellationToken = default)
	{
		var organizationUnits = await IdentityUserRepository.GetOrganizationUnitsAsync(userId, null, includeDetails: false, 0, 1, cancellationToken);
		return organizationUnits.Count > 0 ? new Guid?(((Volo.Abp.Domain.Entities.Entity<Guid>)organizationUnits[0]).Id) : ((Guid?)null);
	}

	public virtual async Task<IReadOnlyList<Guid>> GetMemberUserIdsAsync(Guid organizationUnitId, int skipCount, int maxResultCount, string? filter, CancellationToken cancellationToken = default)
	{
		return (await IdentityUserRepository.GetUsersInOrganizationUnitAsync(organizationUnitId, filter, skipCount, maxResultCount, cancellationToken))
			.Select(user => user.Id)
			.ToArray();
	}
}
