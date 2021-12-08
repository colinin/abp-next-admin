using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    public interface IWorkflowExecutionErrorRepository : IRepository<PersistedExecutionError, int>
    {
    }
}
