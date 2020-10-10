using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.RulesManagement.EntityFrameworkCore
{
    [ConnectionStringName(RulesManagementDbProperties.ConnectionStringName)]
    public class RulesManagementDbContext : AbpDbContext<RulesManagementDbContext>, IRulesManagementDbContext
    {
        public DbSet<EntityRule> EntityRules { get; set; }
        public DbSet<EntityRuleGroup> EntityRuleGroups { get; set; }

        public RulesManagementDbContext(
            DbContextOptions<RulesManagementDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureRulesManagement();
        }
    }
}
