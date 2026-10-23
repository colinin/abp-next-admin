using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.OrganizationUnitManagers;

public class OrganizationUnitManagerService : DomainService
{
	protected IOrganizationUnitManagerRepository Repository { get; }

	protected IOrganizationUnitRepository OrganizationUnitRepository { get; }

	public OrganizationUnitManagerService(IOrganizationUnitManagerRepository repository, IOrganizationUnitRepository organizationUnitRepository)
	{
		Repository = repository;
		OrganizationUnitRepository = organizationUnitRepository;
	}

	public virtual async Task<OrganizationUnitManagerAssignment> AssignAsync(Guid organizationUnitId, Guid userId, bool isPrimary = false, int order = 0, string? remark = null, CancellationToken cancellationToken = default)
	{
		await OrganizationUnitRepository.GetAsync(organizationUnitId, true, cancellationToken);
		if (await Repository.FindByUserAsync(organizationUnitId, userId, cancellationToken) != null)
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.OrganizationUnitManager.AlreadyExists, logLevel: LogLevel.Warning)
				.WithData("OrganizationUnitId", organizationUnitId)
				.WithData("UserId", userId);
		}
		if (isPrimary)
		{
			await DemoteCurrentPrimaryAsync(organizationUnitId, cancellationToken);
		}
		var assignment = new OrganizationUnitManagerAssignment(GuidGenerator.Create(), organizationUnitId, userId, isPrimary, order, remark);
		return await Repository.InsertAsync(assignment, true, cancellationToken);
	}

	public virtual async Task<OrganizationUnitManagerAssignment> SetPrimaryAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var assignment = await Repository.GetAsync(id, true, cancellationToken);
		await DemoteCurrentPrimaryAsync(assignment.OrganizationUnitId, cancellationToken);
		assignment.SetPrimary(isPrimary: true);
		return await Repository.UpdateAsync(assignment, true, cancellationToken);
	}

	public virtual async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var assignment = await Repository.GetAsync(id, true, cancellationToken);
		if (assignment.IsPrimary)
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.OrganizationUnitManager.PrimaryConflict, logLevel: LogLevel.Warning)
				.WithData("OrganizationUnitId", assignment.OrganizationUnitId);
		}
		await Repository.DeleteAsync(assignment, true, cancellationToken);
	}

	public virtual async Task UpdateAsync(Guid id, int order, string? remark, CancellationToken cancellationToken = default)
	{
		var assignment = await Repository.GetAsync(id, true, cancellationToken);
		assignment.ChangeOrder(order);
		assignment.SetRemark(remark);
		await Repository.UpdateAsync(assignment, true, cancellationToken);
	}

	protected virtual async Task DemoteCurrentPrimaryAsync(Guid organizationUnitId, CancellationToken cancellationToken)
	{
		var primary = await Repository.FindPrimaryAsync(organizationUnitId, cancellationToken);
		if (primary != null)
		{
			primary.SetPrimary(isPrimary: false);
			await Repository.UpdateAsync(primary, true, cancellationToken);
		}
	}
}
