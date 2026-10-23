using LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.EntityFrameworkCore.Templates;

public class EfCoreApprovalTemplateRepository : EfCoreRepository<IElsaNextApprovalFlowDbContext, ApprovalTemplate, Guid>, IApprovalTemplateRepository
{
    public EfCoreApprovalTemplateRepository(IDbContextProvider<IElsaNextApprovalFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async virtual Task<ApprovalTemplate?> FindByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var queryable = await GetQueryableAsync();
        return await queryable.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<ApprovalTemplate> specification,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<ApprovalTemplate>> GetListAsync(
        ISpecification<ApprovalTemplate> specification,
        string sorting = $"{nameof(ApprovalTemplate.CreationTime)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .OrderBy(!sorting.IsNullOrWhiteSpace() ? sorting : $"{nameof(ApprovalTemplate.CreationTime)} DESC")
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
