using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.DynamicProxy;

namespace LINGYUN.Abp.Rules
{
    public class EntityChangedRulesInterceptor : AbpInterceptor, ITransientDependency
    {
        protected IRuleFinder RuleFinder { get; }
        protected IEntityRuleContributor EntityRuleContributor { get; }

        public EntityChangedRulesInterceptor(
            IRuleFinder ruleFinder,
            IEntityRuleContributor entityRuleContributor)
        {
            RuleFinder = ruleFinder;
            EntityRuleContributor = entityRuleContributor;
        }

        public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            var entityObj = invocation.Arguments.First();
            // TODO: 针对实体的变更执行一次定义的规则
            // IBasicRepository.InsertAsync || IBasicRepository.UpdateAsync || IBasicRepository.DeleteAsync
            if (entityObj != null && entityObj is IEntity entity)
            {
                await ApplyEntityRuleAsync(entity);
            }

            await invocation.ProceedAsync();
        }

        protected virtual async Task ApplyEntityRuleAsync(IEntity entity)
        {
            Type entityType = ProxyHelper.GetUnProxiedType(entity);
            // 加载规则列表
            var groups = await RuleFinder.GetRuleGroupsAsync(entityType);
            if (groups.Any())
            {
                // 应用规则
                await EntityRuleContributor.ApplyAsync(new EntityRuleContext(groups, entity));
            }
        }
    }
}
