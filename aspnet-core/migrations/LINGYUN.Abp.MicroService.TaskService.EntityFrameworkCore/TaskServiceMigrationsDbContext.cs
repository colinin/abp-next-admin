using LINGYUN.Abp.TaskManagement.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MicroService.TaskService.EntityFrameworkCore;

[ConnectionStringName("TaskManagementDbMigrator")]
public class TaskServiceMigrationsDbContext : AbpDbContext<TaskServiceMigrationsDbContext>
{
    public TaskServiceMigrationsDbContext(DbContextOptions<TaskServiceMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureTaskManagement();
    }
}
