using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.WorkflowManagement.EntityFrameworkCore
{
    [ConnectionStringName(WorkflowManagementDbProperties.ConnectionStringName)]
    public interface IWorkflowManagementDbContext : IEfCoreDbContext
    {
    }
}
