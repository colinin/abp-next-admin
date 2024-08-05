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

    public async virtual Task DeleteAllAsync(
        string sessionId,
        Guid? exceptSessionId = null,
        CancellationToken cancellationToken = default)
    {
        await DeleteAsync(x => x.SessionId == sessionId && x.Id != exceptSessionId, cancellationToken: cancellationToken);
    }
}
