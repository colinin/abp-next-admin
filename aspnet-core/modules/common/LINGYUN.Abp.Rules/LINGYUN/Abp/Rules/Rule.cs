using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Volo.Abp;

namespace LINGYUN.Abp.Rules
{
    /// <summary>
    /// ref: https://github.com/microsoft/RulesEngine/blob/master/src/RulesEngine/RulesEngine/Models/Rule.cs
    /// </summary>
    public class Rule
    {
        [NotNull]
        public string Name { get; }
        public string Operator { get; }
        public string ErrorMessage { get; }
        public DateTime CreationTime { get; }
        public ErrorType ErrorType { get; }
        public ExpressionType? ExpressionType { get; }
        public List<Rule> Rules { get; }
        public List<string> InjectRules { get; }
        public List<RuleParam> Params { get; }
        public string Expression { get; }
        public string SuccessEvent { get; }

        public Rule(
            [NotNull] string name,
            string @operator,
            DateTime creationTime,
            string expression = null,
            string successEvent = null,
            ErrorType errorType = ErrorType.Warning,
            ExpressionType? expressionType = null)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            Name = name;
            Operator = @operator;
            CreationTime = creationTime;
            Expression = expression;
            SuccessEvent = successEvent;
            ErrorType = errorType;
            ExpressionType = expressionType;

            Rules = new List<Rule>();
            Params = new List<RuleParam>();
            InjectRules = new List<string>();
        }

        public Rule CreateChildren(Rule rule)
        {
            Rules.Add(rule);

            return this;
        }

        public Rule WithParam(RuleParam param)
        {
            Params.AddIfNotContains(param);
            return this;
        }

        public Rule InjectRule(string ruleName)
        {
            InjectRules.AddIfNotContains(ruleName);

            return this;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is Rule rule)
            {
                return rule.Name.Equals(Name);
            }

            return false;
        }
    }
}
