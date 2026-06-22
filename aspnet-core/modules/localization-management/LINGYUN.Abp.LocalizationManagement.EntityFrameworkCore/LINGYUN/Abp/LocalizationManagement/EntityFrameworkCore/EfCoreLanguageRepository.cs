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

namespace LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore;

public class EfCoreLanguageRepository : EfCoreRepository<ILocalizationDbContext, Language, Guid>,
    ILanguageRepository
{
    public EfCoreLanguageRepository(
        IDbContextProvider<ILocalizationDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async virtual Task<Language> FindByCultureNameAsync(
        string cultureName,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).Where(x => x.CultureName.Equals(cultureName))
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<Language>> GetActivedListAsync(CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).Where(x => x.Enable)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<Language> specification,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<Language>> GetListAsync(
        ISpecification<Language> specification,
        string sorting = nameof(Language.CultureName),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(specification.ToExpression())
            .OrderBy(!sorting.IsNullOrWhiteSpace() ? sorting : nameof(Language.CultureName))
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
