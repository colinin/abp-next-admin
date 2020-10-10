using JetBrains.Annotations;
using System.Collections.Generic;
using Volo.Abp;

namespace LINGYUN.Abp.Rules
{
    public class RuleGroup
    {
        [NotNull]
        public string Name { get; set; }
        public List<string> InjectRules { get; set; }
        public List<Rule> Rules { get; }
        protected RuleGroup() { }
        public RuleGroup(
            [NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            Name = name;

            Rules = new List<Rule>();
            InjectRules = new List<string>();
        }

        public RuleGroup WithInjectRule(string ruleName)
        {
            InjectRules.AddIfNotContains(ruleName);

            return this;
        }

        public RuleGroup WithInjectRule(IEnumerable<string> ruleNames)
        {
            InjectRules.AddIfNotContains(ruleNames);

            return this;
        }

        public RuleGroup WithRule(Rule rule)
        {
            Rules.AddIfNotContains(rule);

            return this;
        }

        public RuleGroup WithRule(IEnumerable<Rule> rules)
        {
            Rules.AddIfNotContains(rules);

            return this;
        }
    }
}
