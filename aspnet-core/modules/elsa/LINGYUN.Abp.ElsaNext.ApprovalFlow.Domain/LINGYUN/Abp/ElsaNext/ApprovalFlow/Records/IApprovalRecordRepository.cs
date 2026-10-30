using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;

public interface IApprovalRecordRepository : IBasicRepository<ApprovalRecord, Guid>
{
	Task<List<ApprovalRecord>> GetTimelineAsync(
        Guid instanceId, 
        CancellationToken cancellationToken = default);

	Task<List<ApprovalRecord>> GetTimelineListAsync(
        IReadOnlyCollection<Guid> instanceIds, 
        CancellationToken cancellationToken = default);

	Task<bool> ExistsByActionAsync(
        Guid instanceId, 
        string taskId, 
        string actionKey, 
        CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(
        ISpecification<ApprovalRecord> specification,
        CancellationToken cancellationToken = default);

    Task<List<ApprovalRecord>> GetListAsync(
        ISpecification<ApprovalRecord> specification,
        string sorting = $"{nameof(ApprovalRecord.CreationTime)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
