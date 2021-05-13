using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Rules.RulesEngine.Persistent
{
    public class PersistentWorkflowRulesResolveContributor : WorkflowRulesResolveContributorBase, ITransientDependency
    {
        private IWorkflowRuleStore _store;
        public override string Name => "Persistent";

        protected IWorkflowRuleStore Store => _store;

        public PersistentWorkflowRulesResolveContributor()
        {
        }

        public override void Initialize(RulesInitializationContext context)
        {
            _store = context.ServiceProvider.GetRequiredService<IWorkflowRuleStore>();
        }

        public override async Task ResolveAsync(IWorkflowRulesResolveContext context)
        {
            var rules = await Store.GetRulesAsync(context.Type);

            context.Handled = true;
            context.WorkflowRules = rules;
        }
    }
}
