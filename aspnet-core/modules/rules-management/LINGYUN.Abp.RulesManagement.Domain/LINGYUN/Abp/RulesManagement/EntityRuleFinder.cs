using LINGYUN.Abp.Rules;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;

namespace LINGYUN.Abp.RulesManagement
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    [ExposeServices(typeof(IRuleFinder))]
    public class EntityRuleFinder :  DomainService, IRuleFinder
    {
        private IObjectMapper _objectMapper;
        protected IObjectMapper ObjectMapper => LazyGetRequiredService(ref _objectMapper);

        private IEntityRuleGroupRepository _ruleGroupRepository;
        protected IEntityRuleGroupRepository RuleGroupRepository => LazyGetRequiredService(ref _ruleGroupRepository);

        private IEntityRuleRepository _rruleRepository;
        protected IEntityRuleRepository RuleRepository => LazyGetRequiredService(ref _rruleRepository);

        public virtual async Task<List<RuleGroup>> GetRuleGroupsAsync(Type entityType)
        {
            var entityFullTypeName = entityType.FullName;
            if (entityType.IsGenericType)
            {
                entityFullTypeName = entityType.GetGenericTypeDefinition().FullName;
            }
            else if (entityType.IsArray)
            {
                entityFullTypeName = entityType.GetElementType().FullName;
            }
            var entityRuleGroups = await RuleGroupRepository.GetListByTypeAsync(entityFullTypeName, includeDetails: true);

            var ruleGroups = ObjectMapper.Map<List<EntityRuleGroup>, List<RuleGroup>>(entityRuleGroups);

            foreach(var group in ruleGroups)
            {
                var entityRuleGroup = entityRuleGroups.Find(g => g.Name.Equals(group.Name));
                if (entityRuleGroup != null)
                {
                    foreach(var ruleInGroup in entityRuleGroup.Rules)
                    {
                        await AddRuleAsync(group, ruleInGroup.RuleId);
                    }

                    foreach(var ruleInject in entityRuleGroup.InjectRules)
                    {
                        await AddToInjectRuleAsync(group, ruleInject.RuleId);
                    }
                }
            }

            return ruleGroups;
        }

        protected virtual async Task AddRuleAsync(RuleGroup ruleGroup, Guid ruleId)
        {
            var entityRule = await RuleRepository.FindAsync(ruleId);
            if (entityRule == null)
            {
                return;
            }
            var rule = ObjectMapper.Map<EntityRule, Rule>(entityRule);
            ruleGroup.WithRule(rule);

            foreach (var subEntityRule in entityRule.SubRules)
            {
                await AddSubRuleAsync(rule, subEntityRule.SubId);
            }

            foreach (var ruleInject in entityRule.InjectRules)
            {
                // 如果依赖于某个规则,需要把此规则添加到集合
                await AddRuleAsync(ruleGroup, ruleInject.InjectId);

                // 添加依赖规则
                await AddToInjectRuleAsync(ruleGroup, ruleInject.InjectId);
            }
        }

        protected virtual async Task AddSubRuleAsync(Rule rule, Guid subRuleId)
        {
            var entityRule = await RuleRepository.FindAsync(subRuleId);
            if (entityRule == null)
            {
                return;
            }
            var subRule = ObjectMapper.Map<EntityRule, Rule>(entityRule);
            rule.CreateChildren(subRule);
            foreach (var subEntityRule in entityRule.SubRules)
            {
                await AddSubRuleAsync(subRule, subEntityRule.SubId);
            }
        }

        protected virtual async Task AddToInjectRuleAsync(RuleGroup group, Guid ruleId)
        {
            var entityRule = await RuleRepository.FindAsync(ruleId);
            if (entityRule == null)
            {
                return;
            }
            group.WithInjectRule(entityRule.Name);
            foreach (var injectRule in entityRule.InjectRules)
            {
                await AddToInjectRuleAsync(group, injectRule.InjectId);
            }
        }

        protected virtual async Task AddToInjectRuleAsync(Rule rule, Guid ruleId)
        {
            var entityRule = await RuleRepository.FindAsync(ruleId);
            if (entityRule == null)
            {
                return;
            }
            var injectRule = ObjectMapper.Map<EntityRule, Rule>(entityRule);
            rule.CreateChildren(injectRule);
            rule.InjectRule(entityRule.Name);
            foreach (var injectSubRule in entityRule.InjectRules)
            {
                await AddToInjectRuleAsync(injectRule, injectSubRule.InjectId);
            }
        }
    }
}
