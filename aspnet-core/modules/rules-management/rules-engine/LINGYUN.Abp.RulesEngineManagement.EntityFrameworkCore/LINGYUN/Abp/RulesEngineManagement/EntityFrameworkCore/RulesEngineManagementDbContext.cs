using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.RulesEngineManagement.EntityFrameworkCore;

[ConnectionStringName(RulesEngineManagementDbPropertites.ConnectionStringName)]
public class RulesEngineManagementDbContext : AbpDbContext<RulesEngineManagementDbContext>, IRulesEngineManagementDbContext
{
    public DbSet<RuleRecord> RuleRecords { get; set; }

    public DbSet<WorkflowRecord> WorkflowRecords { get; set; }

    public RulesEngineManagementDbContext(
        DbContextOptions<RulesEngineManagementDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureRulesEngineManagement();
    }
}
