using LINGYUN.Abp.PrintDesigner.EntityFrameworkCore;
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

namespace LINGYUN.Abp.PrintDesigner.Templates;

public class EfCoreTemplateRepository : EfCoreRepository<IPrintDesignerDbContext, Template, Guid>, ITemplateRepository
{
    public EfCoreTemplateRepository(
        IDbContextProvider<IPrintDesignerDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async virtual Task<Template?> FindByNameAsync(
        string name,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .IncludeIf(includeDetails, x => x.Data)
            .Where(x => x.Name == name)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<Template> specification,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<Template>> GetListAsync(
        ISpecification<Template> specification,
        string? sorting = nameof(Template.Name),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
              .Where(specification.ToExpression())
              .OrderBy(!sorting.IsNullOrWhiteSpace() ? sorting : nameof(Template.Name))
              .PageBy(skipCount, maxResultCount)
              .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
