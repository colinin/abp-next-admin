using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    public interface IWorkflowScheduledCommandRepository : IRepository<WorkflowScheduledCommand, long>
    {
    }
}
