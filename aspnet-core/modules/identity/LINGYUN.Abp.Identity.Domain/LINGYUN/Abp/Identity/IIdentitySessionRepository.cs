using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity;
public interface IIdentitySessionRepository : Volo.Abp.Identity.IIdentitySessionRepository
{
    Task<IdentitySession> FindLastAsync(
        Guid userId, 
        string device = null, 
        CancellationToken cancellationToken = default);

    Task<List<IdentitySession>> GetListAsync(
        Guid userId,
        string device,
        Guid? exceptSessionId = null,
        int maxResultCount = 0,
        CancellationToken cancellationToken = default);

    Task<List<IdentitySession>> GetListAsync(Guid userId, CancellationToken cancellationToken = default);

    Task DeleteAllAsync(string sessionId, Guid? exceptSessionId = null, CancellationToken cancellationToken = default);
}
