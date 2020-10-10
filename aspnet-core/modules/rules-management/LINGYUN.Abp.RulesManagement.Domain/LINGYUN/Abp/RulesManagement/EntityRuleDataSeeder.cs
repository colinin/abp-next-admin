using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.RulesManagement
{
    public class EntityRuleDataSeeder : IEntityRuleDataSeeder, ITransientDependency
    {
        protected IGuidGenerator GuidGenerator { get; }
        protected ICurrentTenant CurrentTenant { get; }
        protected IEntityRuleGroupRepository Repository { get; }

        public EntityRuleDataSeeder(
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant,
            IEntityRuleGroupRepository repository)
        {
            GuidGenerator = guidGenerator;
            CurrentTenant = currentTenant;
            Repository = repository;
        }

        public virtual async Task SeedAsync(EntityRuleInGroup group)
        {
            var findGroup = await Repository.GetByNameAsync(group.Name);
            if (findGroup != null)
            {
                return;
            }
            await Repository.InsertAsync(group);
        }
    }
}
