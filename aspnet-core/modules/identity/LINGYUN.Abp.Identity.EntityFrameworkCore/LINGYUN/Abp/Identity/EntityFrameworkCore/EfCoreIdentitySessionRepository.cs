using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.Identity.EntityFrameworkCore;

public class EfCoreIdentitySessionRepository : Volo.Abp.Identity.EntityFrameworkCore.EfCoreIdentitySessionRepository, IIdentitySessionRepository
{
    public EfCoreIdentitySessionRepository(
        IDbContextProvider<IIdentityDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async virtual Task<IdentitySession> FindLastAsync(
        Guid userId,
        string device = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.UserId == userId)
            .WhereIf(!device.IsNullOrWhiteSpace(), x => x.Device == device)
            .OrderByDescending(x => x.SignedIn)
            .FirstOrDefaultAsync();
    }

    public async virtual Task<List<IdentitySession>> GetListAsync(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.UserId == userId)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<IdentitySession>> GetListAsync(
        Guid userId,
        string device,
        Guid? exceptSessionId = null,
        int maxResultCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.UserId == userId && x.Device == device && x.Id != exceptSessionId)
            .OrderBy(x => x.SignedIn)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task DeleteAllSessionAsync(
        string sessionId,
        Guid? exceptSessionId = null,
        CancellationToken cancellationToken = default)
    {
        await DeleteAsync(x => x.SessionId == sessionId && x.Id != exceptSessionId, cancellationToken: cancellationToken);
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<IdentitySession> specification,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<IdentitySession>> GetListAsync(
        ISpecification<IdentitySession> specification,
        string sorting = $"{nameof(IdentitySession.SignedIn)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(specification.ToExpression())
            .OrderBy(!sorting.IsNullOrWhiteSpace() ? sorting : $"{nameof(IdentitySession.SignedIn)} DESC")
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
