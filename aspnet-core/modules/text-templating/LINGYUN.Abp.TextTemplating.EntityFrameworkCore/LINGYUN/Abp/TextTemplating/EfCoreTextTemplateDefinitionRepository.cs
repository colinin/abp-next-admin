using LINGYUN.Abp.TextTemplating.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.TextTemplating;
public class EfCoreTextTemplateDefinitionRepository : EfCoreRepository<ITextTemplatingDbContext, TextTemplateDefinition, Guid>, ITextTemplateDefinitionRepository
{
    public EfCoreTextTemplateDefinitionRepository(
        IDbContextProvider<ITextTemplatingDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async virtual Task<TextTemplateDefinition> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.Name == name)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Name.Contains(filter) ||
                x.DefaultCultureName.Contains(filter) || x.Layout.Contains(filter))
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<TextTemplateDefinition>> GetListAsync(
        string filter = null,
        string sorting = "Name", 
        int skipCount = 0, 
        int maxResultCount = 10, 
        CancellationToken cancellationToken = default)
    {
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = nameof(TextTemplateDefinition.Name);
        }

        return await (await GetDbSetAsync())
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Name.Contains(filter) ||
                x.DefaultCultureName.Contains(filter) || x.Layout.Contains(filter))
            .OrderBy(sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
