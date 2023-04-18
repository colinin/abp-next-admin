using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.RulesEngineManagement;
public interface IWorkflowRecordRepository : IRepository<WorkflowRecord, Guid>
{
    Task<WorkflowRecord> FindByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task<List<WorkflowRecord>> GetListAsync(
        string typeFullName,
        CancellationToken cancellationToken = default);

    Task<List<ParamRecord>> GetParamsAsync(
        Guid workflowId, 
        CancellationToken cancellationToken = default);
}
