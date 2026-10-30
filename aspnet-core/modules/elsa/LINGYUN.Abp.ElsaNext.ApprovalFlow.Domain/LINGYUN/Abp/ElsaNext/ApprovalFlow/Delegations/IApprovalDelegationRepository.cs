using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Delegations;

public interface IApprovalDelegationRepository : IBasicRepository<ApprovalDelegation, Guid>
{
	Task<List<ApprovalDelegation>> FindActiveListByDelegatorAsync(
        string delegatorId, 
        DateTimeOffset now, 
        CancellationToken cancellationToken = default);

	Task<List<ApprovalDelegation>> FindActiveListByDelegateAsync(
        string delegateId, 
        DateTimeOffset now, 
        CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(
        ISpecification<ApprovalDelegation> specification,
        CancellationToken cancellationToken = default);

    Task<List<ApprovalDelegation>> GetListAsync(
        ISpecification<ApprovalDelegation> specification,
        string? sorting = $"{nameof(ApprovalDelegation.CreationTime)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
