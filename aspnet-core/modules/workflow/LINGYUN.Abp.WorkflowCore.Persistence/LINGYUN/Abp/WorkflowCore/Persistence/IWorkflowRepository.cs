using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    public interface IWorkflowRepository : IRepository<PersistedWorkflow, Guid>
    {
        Task<List<PersistedWorkflow>> GetListAsync(
            WorkflowStatus? status,
            string type,
            DateTime? createdFrom,
            DateTime? createdTo,
            int skip,
            int take,
            CancellationToken cancellationToken = default);

        Task<List<PersistedWorkflow>> GetOlderListAsync(
            WorkflowStatus status,
            DateTime olderThan,
            bool includeDetails = false,
            int? maxResultCount = null,
            CancellationToken cancellationToken = default);
    }
}
