using LINGYUN.Abp.TaskManagement;
using LINGYUN.Abp.TaskManagement.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MicroService.TaskService;

[ConnectionStringName("Default")]
public class TaskServiceMigrationsDbContext : 
    AbpDbContext<TaskServiceMigrationsDbContext>,
    ITaskManagementDbContext
{
    public DbSet<BackgroundJobInfo> BackgroundJobInfos { get; set; }

    public DbSet<BackgroundJobLog> BackgroundJobLogs { get; set; }

    public DbSet<BackgroundJobAction> BackgroundJobAction { get; set; }

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
