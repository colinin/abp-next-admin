using LINGYUN.Abp.AIManagement.Workspaces;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.AIManagement.EntityFrameworkCore;

[ConnectionStringName(AbpAIManagementDbProperties.ConnectionStringName)]
public interface IAIManagementDbContext : IEfCoreDbContext
{
    DbSet<WorkspaceDefinitionRecord> Workspaces { get; }
}
