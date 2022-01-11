using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.TaskManagement.EntityFrameworkCore;

[ConnectionStringName(TaskManagementDbProperties.ConnectionStringName)]
public interface ITaskManagementDbContext :IEfCoreDbContext
{
}
