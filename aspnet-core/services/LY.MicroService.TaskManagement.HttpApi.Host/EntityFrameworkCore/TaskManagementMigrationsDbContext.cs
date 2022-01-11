using LINGYUN.Abp.TaskManagement.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LY.MicroService.TaskManagement.EntityFrameworkCore;

public class TaskManagementMigrationsDbContext : AbpDbContext<TaskManagementMigrationsDbContext>
{
    public TaskManagementMigrationsDbContext(DbContextOptions<TaskManagementMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureTaskManagement();
    }
}
