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
using Volo.Abp.Threading;

namespace LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore;

public class EfCoreResourceRepository : EfCoreRepository<ILocalizationDbContext, Resource, Guid>,
    IResourceRepository
{
    public EfCoreResourceRepository(
        IDbContextProvider<ILocalizationDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async virtual Task<bool> ExistsAsync(
        string name, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).AnyAsync(x => x.Name.Equals(name));
    }

    [Obsolete("Use FindAsync() method.")]
    public virtual Resource FindByName(string name)
    {
        using (Volo.Abp.Uow.UnitOfWorkManager.DisableObsoleteDbContextCreationWarning.SetScoped(true))
        {
            return DbSet.FirstOrDefault(localizationResourceRecord => localizationResourceRecord.Name == name);
        }
    }

    public async virtual Task<Resource> FindByNameAsync(
        string name, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).Where(x => x.Name.Equals(name))
          .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<Resource>> GetActivedListAsync(CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).Where(x => x.Enable)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<Resource> specification,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<Resource>> GetListAsync(
        ISpecification<Resource> specification,
        string sorting = nameof(Resource.Name),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(specification.ToExpression())
            .OrderBy(!sorting.IsNullOrWhiteSpace() ? sorting : nameof(Resource.Name))
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
