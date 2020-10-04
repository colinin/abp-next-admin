using JetBrains.Annotations;
using System.Collections.Generic;
using Volo.Abp;

namespace LINGYUN.Abp.Rules
{
    public class RuleGroup
    {
        [NotNull]
        public string Name { get; }
        public List<string> InjectRules { get; }
        public List<Rule> Rules { get; }

        public RuleGroup(
            [NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            Name = name;

            Rules = new List<Rule>();
            InjectRules = new List<string>();
        }

        public RuleGroup InjectRule(string ruleName)
        {
            InjectRules.AddIfNotContains(ruleName);

            return this;
        }

        public RuleGroup WithRule(Rule rule)
        {
            Rules.AddIfNotContains(rule);

            return this;
        }
    }
}
