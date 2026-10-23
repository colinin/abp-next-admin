using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.CarbonCopies;

public interface IApprovalCcRepository : IBasicRepository<ApprovalCc, Guid>
{
    Task<int> GetCountAsync(
        ISpecification<ApprovalCc> specification,
        CancellationToken cancellationToken = default);

    Task<List<ApprovalCc>> GetListAsync(
        ISpecification<ApprovalCc> specification,
        string? sorting = nameof(ApprovalCc.NotifiedAt),
        int maxResultCount = 10,
        int skipCount = 0, 
        CancellationToken cancellationToken = default);

	Task<List<ApprovalCc>> GetListByInstanceAsync(
        Guid instanceId,
        CancellationToken cancellationToken = default);
}
