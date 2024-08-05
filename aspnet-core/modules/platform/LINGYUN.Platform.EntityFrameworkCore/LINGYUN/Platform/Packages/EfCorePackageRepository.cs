using LINGYUN.Platform.EntityFrameworkCore;
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

namespace LINGYUN.Platform.Packages;
public class EfCorePackageRepository :
    EfCoreRepository<PlatformDbContext, Package, Guid>,
    IPackageRepository
{
    public EfCorePackageRepository(
        IDbContextProvider<PlatformDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async virtual Task<Package> FindByNameAsync(
        string name,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(x => x.Name == name)
            .OrderByDescending(x => x.Version)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<Package> FindLatestAsync(
        string name,
        string version = null,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        if (version.IsNullOrWhiteSpace())
        {
            return await (await GetDbSetAsync())
                .IncludeDetails(includeDetails)
                .Where(x => x.Name == name)
                .OrderByDescending(x => x.Version)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(x => x.Name == name)
            .OrderByDescending(x => x.Level)
            .ThenByDescending(x => x.Version)
            .Where(x => x.Version.CompareTo(version) > 0)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<int> GetCountAsync(
        Specification<Package> specification, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<Package>> GetListAsync(
        Specification<Package> specification,
        string sorting = $"{nameof(Package.Version)} DESC",
        int skipCount = 0, 
        int maxResultCount = 10, 
        CancellationToken cancellationToken = default)
    {
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = $"{nameof(Package.Version)} DESC";
        }

        return await (await GetDbSetAsync())
            .Where(specification.ToExpression())
            .OrderBy(sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async override Task<IQueryable<Package>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}
