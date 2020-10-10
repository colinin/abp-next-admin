using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.RulesManagement.EntityFrameworkCore
{
    [ConnectionStringName(RulesManagementDbProperties.ConnectionStringName)]
    public interface IRulesManagementDbContext : IEfCoreDbContext
    {
        DbSet<EntityRule> EntityRules { get; set; }
        DbSet<EntityRuleGroup> EntityRuleGroups { get; set; }
    }
}
