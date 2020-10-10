using System.Threading.Tasks;

namespace LINGYUN.Abp.RulesManagement
{
    public interface IEntityRuleDataSeeder
    {
        Task SeedAsync(EntityRuleInGroup group);
    }
}
