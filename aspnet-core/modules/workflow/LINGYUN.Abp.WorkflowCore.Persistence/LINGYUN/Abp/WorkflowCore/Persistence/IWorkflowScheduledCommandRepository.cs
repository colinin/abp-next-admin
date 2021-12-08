using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.WorkflowCore.Persistence
{
    public interface IWorkflowScheduledCommandRepository : IRepository<PersistedScheduledCommand, long>
    {
        Task<bool> CheckExistsAsync(
            string name,
            string data,
            CancellationToken cancellationToken = default);
    }
}
