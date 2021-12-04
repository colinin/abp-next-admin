using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.WorkflowCore.Persistence.EntityFrameworkCore
{
    [ConnectionStringName(WorkflowDbProperties.ConnectionStringName)]
    public interface IWorkflowDbContext : IEfCoreDbContext
    {
        DbSet<Workflow> Workflows { get; set; }
        DbSet<WorkflowEvent> WorkflowEvents { get; set; }
        DbSet<WorkflowEventSubscription> WorkflowEventSubscriptions { get; set; }
        DbSet<WorkflowExecutionError> WorkflowExecutionErrors { get; set; }
        DbSet<WorkflowExecutionPointer> WorkflowExecutionPointers { get; set; }
        DbSet<WorkflowExtensionAttribute> WorkflowExtensionAttributes { get; set; }
        DbSet<WorkflowScheduledCommand> WorkflowScheduledCommands { get; set; }
    }
}
