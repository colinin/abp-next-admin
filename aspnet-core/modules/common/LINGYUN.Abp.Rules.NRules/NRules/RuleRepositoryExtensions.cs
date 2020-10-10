using NRules;
using System.Collections.Generic;

namespace LINGYUN.Abp.Rules
{
    public static class RuleRepositoryExtensions
    {
        public static ISessionFactory Compile(this INRulesRepository repository, IEnumerable<RuleGroup> groups)
        {
            var compiler = new RuleCompiler();
            ISessionFactory factory = compiler.Compile(repository.GetRuleSets(groups));
            return factory;
        }
    }
}
