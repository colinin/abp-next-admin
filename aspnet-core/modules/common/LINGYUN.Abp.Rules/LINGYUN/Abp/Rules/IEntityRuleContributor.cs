using System.Threading.Tasks;

namespace LINGYUN.Abp.Rules
{
    public interface IEntityRuleContributor
    {
        Task ApplyAsync(EntityRuleContext context);
    }
}
