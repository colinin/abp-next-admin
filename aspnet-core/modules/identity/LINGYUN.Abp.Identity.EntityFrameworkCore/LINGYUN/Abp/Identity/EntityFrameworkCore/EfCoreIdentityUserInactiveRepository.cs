using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.Identity.EntityFrameworkCore;

public class EfCoreIdentityUserInactiveRepository : EfCoreRepository<IIdentityDbContext, IdentityUserInactive, long>, IIdentityUserInactiveRepository
{
    public EfCoreIdentityUserInactiveRepository(
        IDbContextProvider<IIdentityDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async virtual Task<IdentityUserInactive> FindByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<int> GetInactiveUserCountAsync(
        DateTime threshold,
        IEnumerable<Guid> exceptUserIds = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var ignoreUserIds = dbContext.Set<IdentityUserInactive>()
            .Select(x => x.UserId);

        return await dbContext.Set<IdentityUser>()
            .WhereIf(exceptUserIds?.Count() > 0, x => !exceptUserIds.Contains(x.Id))
            .Where(x => !ignoreUserIds.Contains(x.Id))
            .Where(x => x.IsActive &&
                ((x.LastSignInTime.HasValue && x.LastSignInTime < threshold) ||
                 // 更新到abp新版后用户未登陆过使用上次修改时间作为判断依据?
                 // (x.LastModificationTime.HasValue && x.LastModificationTime < threshold) ||
                 x.CreationTime < threshold))
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<IdentityUser>> GetInactiveUserListAsync(
        DateTime threshold,
        IEnumerable<Guid> exceptUserIds = null,
        string sorting = nameof(IdentityUser.LastSignInTime),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var ignoreUserIds = dbContext.Set<IdentityUserInactive>()
            .Select(x => x.UserId);

        return await dbContext.Set<IdentityUser>()
            .WhereIf(exceptUserIds?.Count() > 0, x => !exceptUserIds.Contains(x.Id))
            .Where(x => !ignoreUserIds.Contains(x.Id))
            .Where(x => x.IsActive &&
                ((x.LastSignInTime.HasValue && x.LastSignInTime < threshold) ||
                 // 更新到abp新版后用户未登陆过使用上次修改时间作为判断依据?
                 // (x.LastModificationTime.HasValue && x.LastModificationTime < threshold) ||
                 x.CreationTime < threshold))
            .OrderBy(!sorting.IsNullOrWhiteSpace() ? sorting : nameof(IdentityUser.LastSignInTime))
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<IdentityUserInactive> specification,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<IdentityUserInactive>> GetListAsync(
        ISpecification<IdentityUserInactive> specification,
        string sorting = nameof(IdentityUserInactive.CreationTime),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .OrderBy(!sorting.IsNullOrWhiteSpace() ? sorting : nameof(IdentityUserInactive.CreationTime))
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
