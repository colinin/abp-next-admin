using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.TaskManagement.EntityFrameworkCore;

[ConnectionStringName(TaskManagementDbProperties.ConnectionStringName)]
public interface ITaskManagementDbContext :IEfCoreDbContext
{
    DbSet<BackgroundJobInfo> BackgroundJobInfos { get; }
    DbSet<BackgroundJobLog> BackgroundJobLogs { get; }
    DbSet<BackgroundJobAction> BackgroundJobAction { get; }
}
