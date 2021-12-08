using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.WorkflowCore.Persistence.EntityFrameworkCore
{
    [ConnectionStringName(WorkflowDbProperties.ConnectionStringName)]
    public interface IWorkflowDbContext : IEfCoreDbContext
    {
        DbSet<PersistedWorkflow> Workflows { get; set; }
        DbSet<PersistedEvent> WorkflowEvents { get; set; }
        DbSet<PersistedSubscription> WorkflowEventSubscriptions { get; set; }
        DbSet<PersistedExecutionError> WorkflowExecutionErrors { get; set; }
        DbSet<PersistedExecutionPointer> WorkflowExecutionPointers { get; set; }
        DbSet<PersistedExtensionAttribute> WorkflowExtensionAttributes { get; set; }
        DbSet<PersistedScheduledCommand> WorkflowScheduledCommands { get; set; }
    }
}
