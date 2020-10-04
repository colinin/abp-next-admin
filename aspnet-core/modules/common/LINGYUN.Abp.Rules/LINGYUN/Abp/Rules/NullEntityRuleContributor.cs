using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Rules
{
    public class NullEntityRuleContributor : IEntityRuleContributor, ISingletonDependency
    {
        public Task ApplyAsync(EntityRuleContext context)
        {
            return Task.CompletedTask;
        }
    }
}
