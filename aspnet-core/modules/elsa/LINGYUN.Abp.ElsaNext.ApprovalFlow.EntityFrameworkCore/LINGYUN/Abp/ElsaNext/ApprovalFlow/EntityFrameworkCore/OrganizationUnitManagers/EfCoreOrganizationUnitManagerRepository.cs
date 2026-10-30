using LINGYUN.Abp.ElsaNext.ApprovalFlow.OrganizationUnitManagers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.EntityFrameworkCore.OrganizationUnitManagers;

public class EfCoreOrganizationUnitManagerRepository : EfCoreRepository<IElsaNextApprovalFlowDbContext, OrganizationUnitManagerAssignment, Guid>, IOrganizationUnitManagerRepository
{
	public EfCoreOrganizationUnitManagerRepository(IDbContextProvider<IElsaNextApprovalFlowDbContext> dbContextProvider)
		: base(dbContextProvider)
	{
	}

	public virtual async Task<List<OrganizationUnitManagerAssignment>> GetListByOrganizationUnitAsync(Guid organizationUnitId, CancellationToken cancellationToken = default)
	{
		return await (await GetQueryableAsync())
			.Where(x => x.OrganizationUnitId == organizationUnitId)
			.OrderByDescending(x => x.IsPrimary)
			.ThenBy(x => x.Order)
			.ToListAsync(GetCancellationToken(cancellationToken));
	}

	public virtual async Task<List<OrganizationUnitManagerAssignment>> GetListByUserAsync(Guid userId, CancellationToken cancellationToken = default)
	{
		return await (await GetQueryableAsync())
			.Where(x => x.UserId == userId)
			.OrderBy(x => x.OrganizationUnitId)
			.ToListAsync(GetCancellationToken(cancellationToken));
	}

	public virtual async Task<OrganizationUnitManagerAssignment?> FindByUserAsync(Guid organizationUnitId, Guid userId, CancellationToken cancellationToken = default)
	{
		return await (await GetQueryableAsync())
			.FirstOrDefaultAsync(x => x.OrganizationUnitId == organizationUnitId && x.UserId == userId, GetCancellationToken(cancellationToken));
	}

	public virtual async Task<OrganizationUnitManagerAssignment?> FindPrimaryAsync(Guid organizationUnitId, CancellationToken cancellationToken = default)
	{
		return await (await GetQueryableAsync())
			.FirstOrDefaultAsync(x => x.OrganizationUnitId == organizationUnitId && x.IsPrimary, GetCancellationToken(cancellationToken));
	}

	public virtual async Task<List<OrganizationUnitManagerAssignment>> GetPrimaryListAsync(IReadOnlyCollection<Guid> organizationUnitIds, CancellationToken cancellationToken = default)
	{
		var idList = organizationUnitIds.ToList();
		if (idList.Count == 0)
		{
			return new List<OrganizationUnitManagerAssignment>();
		}

		return await (await GetQueryableAsync())
			.Where(x => idList.Contains(x.OrganizationUnitId) && x.IsPrimary)
			.ToListAsync(GetCancellationToken(cancellationToken));
	}

	public virtual async Task<List<OrganizationUnitManagerAssignment>> GetPagedListAsync(Guid? organizationUnitId, Guid? userId, string? sorting, int skipCount, int maxResultCount, CancellationToken cancellationToken = default)
	{
		var queryable = await GetQueryableAsync();
		if (organizationUnitId.HasValue)
		{
			queryable = queryable.Where(x => x.OrganizationUnitId == organizationUnitId.Value);
		}
		if (userId.HasValue)
		{
			queryable = queryable.Where(x => x.UserId == userId.Value);
		}

		var sortingText = sorting?.Trim().ToLowerInvariant();
		IOrderedQueryable<OrganizationUnitManagerAssignment> orderedQueryable = sortingText switch
		{
			"order" => queryable.OrderBy(x => x.Order),
			"order desc" => queryable.OrderByDescending(x => x.Order),
			"creationtime" => queryable.OrderBy(x => x.CreationTime),
			"creationtime desc" => queryable.OrderByDescending(x => x.CreationTime),
			_ => queryable.OrderBy(x => x.OrganizationUnitId).ThenBy(x => x.Order).ThenBy(x => x.CreationTime),
		};

		return await orderedQueryable
			.Skip(skipCount)
			.Take(maxResultCount)
			.ToListAsync(GetCancellationToken(cancellationToken));
	}

	public virtual async Task<long> GetCountAsync(Guid? organizationUnitId, Guid? userId, CancellationToken cancellationToken = default)
	{
		var queryable = await GetQueryableAsync();
		if (organizationUnitId.HasValue)
		{
			queryable = queryable.Where(x => x.OrganizationUnitId == organizationUnitId.Value);
		}
		if (userId.HasValue)
		{
			queryable = queryable.Where(x => x.UserId == userId.Value);
		}

		return await queryable.LongCountAsync(GetCancellationToken(cancellationToken));
	}
}
