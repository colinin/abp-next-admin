using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.Specifications;

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

    Task DeleteAllSessionAsync(string sessionId, Guid? exceptSessionId = null, CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(
        ISpecification<IdentitySession> specification,
        CancellationToken cancellationToken = default);

    Task<List<IdentitySession>> GetListAsync(
        ISpecification<IdentitySession> specification,
        string sorting = $"{nameof(IdentitySession.SignedIn)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
