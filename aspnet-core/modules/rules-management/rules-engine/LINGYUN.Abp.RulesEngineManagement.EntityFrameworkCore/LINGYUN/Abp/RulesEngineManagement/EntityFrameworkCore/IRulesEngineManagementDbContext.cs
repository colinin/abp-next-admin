using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.RulesEngineManagement.EntityFrameworkCore;

[ConnectionStringName(RulesEngineManagementDbPropertites.ConnectionStringName)]
public interface IRulesEngineManagementDbContext : IEfCoreDbContext
{
    DbSet<RuleRecord> RuleRecords { get; }
    DbSet<WorkflowRecord> WorkflowRecords { get; }
}
