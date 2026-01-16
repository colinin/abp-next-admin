using LINGYUN.Abp.AIManagement.Workspaces;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.AIManagement.EntityFrameworkCore;

[ConnectionStringName(AbpAIManagementDbProperties.ConnectionStringName)]
public class AIManagementDbContext : AbpDbContext<AIManagementDbContext>, IAIManagementDbContext
{
    public DbSet<WorkspaceDefinitionRecord> Workspaces { get; set; }
    public AIManagementDbContext(
        DbContextOptions<AIManagementDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureAIManagement();
    }
}
