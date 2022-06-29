using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LY.MicroService.WorkflowManagement.EntityFrameworkCore;

public class WorkflowManagementMigrationsDbContext : AbpDbContext<WorkflowManagementMigrationsDbContext>
{
    public WorkflowManagementMigrationsDbContext(DbContextOptions<WorkflowManagementMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

       // modelBuilder.ConfigureWorkflow();
        //modelBuilder.ConfigureWorkflowManagement();
    }
}
