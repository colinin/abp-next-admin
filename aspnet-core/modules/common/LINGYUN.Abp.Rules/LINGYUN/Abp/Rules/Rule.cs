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
        public string Name { get; set; }
        public string Operator { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime CreationTime { get; set; }
        public ErrorType ErrorType { get; set; }
        public ExpressionType? ExpressionType { get; set; }
        public List<Rule> Rules { get; set; }
        public List<string> InjectRules { get; set; }
        public List<RuleParam> Params { get; set; }
        public string Expression { get; set; }
        public string SuccessEvent { get; set; }
        protected Rule() { }
        public Rule(
            [NotNull] string name,
            string operation,
            DateTime creationTime,
            string expression = null,
            string successEvent = null,
            ErrorType errorType = ErrorType.Warning,
            ExpressionType? expressionType = null)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            Name = name;
            Operator = operation;
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
