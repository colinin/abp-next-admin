using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.WorkflowCore.Persistence.EntityFrameworkCore
{
    [ConnectionStringName(WorkflowDbProperties.ConnectionStringName)]
    public class WorkflowDbContext : AbpDbContext<WorkflowDbContext>, IWorkflowDbContext
    {
        public virtual DbSet<Workflow> Workflows { get; set; }
        public virtual DbSet<WorkflowEvent> WorkflowEvents { get; set; }
        public virtual DbSet<WorkflowEventSubscription> WorkflowEventSubscriptions { get; set; }
        public virtual DbSet<WorkflowExecutionError> WorkflowExecutionErrors { get; set; }
        public virtual DbSet<WorkflowExecutionPointer> WorkflowExecutionPointers { get; set; }
        public virtual DbSet<WorkflowExtensionAttribute> WorkflowExtensionAttributes { get; set; }
        public virtual DbSet<WorkflowScheduledCommand> WorkflowScheduledCommands { get; set; }

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
