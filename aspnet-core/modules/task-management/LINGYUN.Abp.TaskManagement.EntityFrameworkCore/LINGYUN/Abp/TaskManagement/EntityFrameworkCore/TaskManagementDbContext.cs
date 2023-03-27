using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.TaskManagement.EntityFrameworkCore;

[ConnectionStringName(TaskManagementDbProperties.ConnectionStringName)]
public class TaskManagementDbContext : AbpDbContext<TaskManagementDbContext>, ITaskManagementDbContext
{
    public DbSet<BackgroundJobInfo> BackgroundJobInfos { get; set; }
    public DbSet<BackgroundJobAction> BackgroundJobAction { get; set; }

    public TaskManagementDbContext(
        DbContextOptions<TaskManagementDbContext> options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureTaskManagement();
    }
}
