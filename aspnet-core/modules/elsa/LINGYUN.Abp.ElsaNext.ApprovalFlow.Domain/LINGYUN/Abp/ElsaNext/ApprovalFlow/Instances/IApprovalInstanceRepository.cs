using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;

public interface IApprovalInstanceRepository : IBasicRepository<ApprovalInstance, Guid>
{
    Task<int> GetCountAsync(
        ISpecification<ApprovalInstance> specification,
        CancellationToken cancellationToken = default);

    Task<List<ApprovalInstance>> GetListAsync(
        ISpecification<ApprovalInstance> specification,
        string? sorting = $"{nameof(ApprovalInstance.CreationTime)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);

	Task<ApprovalInstance?> FindByElsaWorkflowInstanceIdAsync(
        string elsaWorkflowInstanceId, 
        CancellationToken cancellationToken = default);
}
