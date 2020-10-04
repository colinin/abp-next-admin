using JetBrains.Annotations;
using Volo.Abp;

namespace LINGYUN.Abp.Rules
{
    public class RuleParam
    {
        [NotNull]
        public string Name { get; }

        [NotNull]
        public string Expression { get; }
        public RuleParam(
            [NotNull] string name,
            [NotNull] string expression)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNullOrWhiteSpace(expression, nameof(expression));

            Name = name;
            Expression = expression;
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

            if (obj is RuleParam param)
            {
                return param.Name.Equals(Name);
            }
            return base.Equals(obj);
        }
    }
}
