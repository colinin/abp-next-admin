using Microsoft.Extensions.DependencyInjection;
using NRules.RuleModel;
using NRules.RuleModel.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Rules
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    [ExposeServices(typeof(IEntityRuleContributor))]
    public class NRulesEntityRuleContributor : IEntityRuleContributor
    {
        protected INRulesRepository Repository { get; }
        public Task ApplyAsync(EntityRuleContext context)
        {
            var entityType = context.Entity.GetType();
            var entityRuleName = RuleNameAttribute.GetRuleName(entityType);

            

            IEnumerable<IRuleSet> groupRuleSets = new List<IRuleSet>();

            groupRuleSets = Repository.GetRuleSets(context.Groups);

            var sessionFactory = Repository.Compile(context.Groups);
            var session = sessionFactory.CreateSession();
            session.Insert(context.Entity);

            session.Fire();

            foreach (var groupRuleSet in groupRuleSets)
            {
            }

            foreach (var group in context.Groups)
            {
                var groupRuleSet = new RuleSet(group.Name);

                Repository.GetRuleSet(group.Name);

                foreach (var rule in group.Rules)
                {
                    var builder = new RuleBuilder();
                    builder.Name(rule.Name);

                    PatternBuilder thisRulePattern = builder.LeftHandSide().Pattern(entityType, entityRuleName);
                    ParameterExpression thisRuleParameter = thisRulePattern.Declaration.ToParameterExpression();
                    var ruleCondition = Expression.Lambda(DynamicExpressionParser.ParseLambda(typeof(bool), rule.Expression), thisRuleParameter);
                    thisRulePattern.Condition(ruleCondition);
                }
            }
            throw new NotImplementedException();
        }
    }
}
