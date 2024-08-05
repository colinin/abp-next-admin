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

namespace LINGYUN.Platform.Layouts;

public class EfCoreLayoutRepository : EfCoreRepository<PlatformDbContext, Layout, Guid>, ILayoutRepository
{
    public EfCoreLayoutRepository(IDbContextProvider<PlatformDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async virtual Task<Layout> FindByNameAsync(
        string name,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .Where(x => x.Name == name)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<int> GetCountAsync(
        string framework = "",
        string filter = "", 
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .WhereIf(!framework.IsNullOrWhiteSpace(), x => x.Framework.Equals(framework))
            .WhereIf(!filter.IsNullOrWhiteSpace(), x =>
                    x.Name.Contains(filter) || x.DisplayName.Contains(filter) ||
                    x.Description.Contains(filter) || x.Redirect.Contains(filter))
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<Layout>> GetPagedListAsync(
        string framework = "",
        string filter = "",
        string sorting = nameof(Layout.Name),
        bool includeDetails = false, 
        int skipCount = 0, 
        int maxResultCount = 10,
        CancellationToken cancellationToken = default)
    {
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = nameof(Layout.Name);
        }

        return await (await GetDbSetAsync())
            .IncludeDetails(includeDetails)
            .WhereIf(!framework.IsNullOrWhiteSpace(), x => x.Framework.Equals(framework))
            .WhereIf(!filter.IsNullOrWhiteSpace(), x =>
                    x.Name.Contains(filter) || x.DisplayName.Contains(filter) ||
                    x.Description.Contains(filter) || x.Redirect.Contains(filter))
            .OrderBy(sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public override async Task<IQueryable<Layout>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }

    [System.Obsolete("将在abp框架移除之后删除")]
    public override IQueryable<Layout> WithDetails()
    {
        return GetQueryable().IncludeDetails();
    }
}
