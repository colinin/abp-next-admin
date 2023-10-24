using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Rules.RulesEngine.Persistent
{
    public class PersistentWorkflowsResolveContributor : WorkflowsResolveContributorBase, ITransientDependency
    {
        public override string Name => "Persistent";

        public PersistentWorkflowsResolveContributor()
        {
        }

        public async override Task ResolveAsync(IWorkflowsResolveContext context)
        {
            var store = context.ServiceProvider.GetRequiredService<IWorkflowStore>();
            var workflows = await store.GetWorkflowsAsync(context.Type);

            //foreach (var workflow in workflows )
            //{
            //    if (workflow.WorkflowsToInject != null)
            //    {
            //        foreach (var injectWorkflow in workflow.WorkflowsToInject)
            //        {
            //        }
            //    }
            //}

            context.Handled = true;
            context.Workflows = workflows;
        }
    }
}
