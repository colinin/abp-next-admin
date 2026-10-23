using LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.EntityFrameworkCore.FormDefinitions;

public class EfCoreFormDefinitionRepository : EfCoreRepository<IElsaNextApprovalFlowDbContext, FormDefinition, Guid>, IFormDefinitionRepository
{
	public EfCoreFormDefinitionRepository(IDbContextProvider<IElsaNextApprovalFlowDbContext> dbContextProvider)
		: base(dbContextProvider)
	{
	}

	public async virtual Task<FormDefinition?> FindByCodeAsync(
        string code, 
        int? version = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
	{
		return await (await GetQueryableAsync())
            .IncludeIf(includeDetails, x => x.Fields)
            .Where(x => x.Code == code)
            .WhereIf(version.HasValue, x => x.Version == version)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<FormDefinition> specification,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<FormDefinition>> GetListAsync(
        ISpecification<FormDefinition> specification,
        string? sorting = $"{nameof(FormDefinition.CreationTime)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .OrderBy(!sorting.IsNullOrWhiteSpace() ? sorting : $"{nameof(FormDefinition.CreationTime)} DESC")
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async override Task<IQueryable<FormDefinition>> WithDetailsAsync()
    {
        return (await base.WithDetailsAsync()).Include(x => x.Fields);
    }
}
