using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.Identity;

public interface IIdentityUserInactiveRepository : IBasicRepository<IdentityUserInactive, long>
{
    Task<IdentityUserInactive> FindByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<int> GetInactiveUserCountAsync(
        DateTime threshold,
        IEnumerable<Guid> exceptUserIds = null,
        CancellationToken cancellationToken = default);

    Task<List<IdentityUser>> GetInactiveUserListAsync(
        DateTime threshold,
        IEnumerable<Guid> exceptUserIds = null,
        string sorting = nameof(IdentityUser.LastSignInTime),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(
        ISpecification<IdentityUserInactive> specification,
        CancellationToken cancellationToken = default);

    Task<List<IdentityUserInactive>> GetListAsync(
        ISpecification<IdentityUserInactive> specification,
        string sorting = nameof(IdentityUserInactive.CreationTime),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
