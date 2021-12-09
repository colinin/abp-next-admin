using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.WorkflowManagement.EntityFrameworkCore
{
    public class WorkflowManagementDbContext : AbpDbContext<WorkflowManagementDbContext>, IWorkflowManagementDbContext
    {
        public WorkflowManagementDbContext(
            DbContextOptions<WorkflowManagementDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureWorkflowManagement();
        }
    }
}
