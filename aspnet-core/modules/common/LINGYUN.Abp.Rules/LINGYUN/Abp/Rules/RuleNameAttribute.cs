using System;
using System.Reflection;

namespace LINGYUN.Abp.Rules
{
    public class RuleNameAttribute : Attribute
    {
        public string Name { get; }
        public RuleNameAttribute(string name)
        {
            Name = name;
        }

        public virtual string GetRuleNameForType(Type ruleType)
        {
            return Name;
        }

        public static string GetRuleName<TEntity>()
        {
            return GetRuleName(typeof(TEntity));
        }

        public static string GetRuleName(Type entityType)
        {
            var ruleAttribute = entityType.GetSingleAttributeOrNull<RuleNameAttribute>();
            if (ruleAttribute != null)
            {
                return ruleAttribute.GetRuleNameForType(entityType);
            }

            return entityType.Name.RemovePostFix("Rule").ToKebabCase();
        }
    }
}
