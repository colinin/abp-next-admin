using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    public interface IWorkflowRepository : IRepository<Workflow, Guid>
    {
        Task<List<Workflow>> GetListAsync(
            WorkflowStatus? status,
            string type,
            DateTime? createdFrom,
            DateTime? createdTo,
            int skip,
            int take,
            CancellationToken cancellationToken = default);
    }
}
