using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.WorkflowCore.Persistence.EntityFrameworkCore
{
    [ConnectionStringName(WorkflowDbProperties.ConnectionStringName)]
    public class WorkflowDbContext : AbpDbContext<WorkflowDbContext>, IWorkflowDbContext
    {
        public virtual DbSet<PersistedWorkflow> Workflows { get; set; }
        public virtual DbSet<PersistedEvent> WorkflowEvents { get; set; }
        public virtual DbSet<PersistedSubscription> WorkflowEventSubscriptions { get; set; }
        public virtual DbSet<PersistedExecutionError> WorkflowExecutionErrors { get; set; }
        public virtual DbSet<PersistedExecutionPointer> WorkflowExecutionPointers { get; set; }
        public virtual DbSet<PersistedExtensionAttribute> WorkflowExtensionAttributes { get; set; }
        public virtual DbSet<PersistedScheduledCommand> WorkflowScheduledCommands { get; set; }

        public WorkflowDbContext(DbContextOptions<WorkflowDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureWorkflow();
        }
    }
}
