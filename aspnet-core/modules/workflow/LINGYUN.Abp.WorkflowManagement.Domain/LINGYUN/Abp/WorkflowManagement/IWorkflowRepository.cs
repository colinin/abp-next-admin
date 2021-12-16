using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.WorkflowManagement
{
    public interface IWorkflowRepository : IRepository<Workflow, Guid>
    {
        Task<bool> CheckVersionAsync(
            string name,
            int version,
            CancellationToken cancellationToken = default);
    }
}
