using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    public class WorkflowRulesResolver : IWorkflowRulesResolver, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AbpRulesEngineResolveOptions _options;

        public WorkflowRulesResolver(
            IServiceProvider serviceProvider,
            IOptions<AbpRulesEngineResolveOptions> options)
        {
            _options = options.Value;
            _serviceProvider = serviceProvider;
        }

        public virtual void Initialize(RulesInitializationContext context)
        {
            foreach (var workflowRulesResolver in _options.WorkflowRulesResolvers)
            {
                workflowRulesResolver.Initialize(context);
            }
        }

        public virtual async Task<WorkflowRulesResolveResult> ResolveWorkflowRulesAsync(Type type)
        {
            var result = new WorkflowRulesResolveResult();

            using (var serviceScope = _serviceProvider.CreateScope())
            {
                var context = new WorkflowRulesResolveContext(type, serviceScope.ServiceProvider);

                foreach (var workflowRulesResolver in _options.WorkflowRulesResolvers)
                {
                    await workflowRulesResolver.ResolveAsync(context);

                    result.AppliedResolvers.Add(workflowRulesResolver.Name);

                    if (context.HasResolved())
                    {
                        result.WorkflowRules.AddRange(context.WorkflowRules);

                        if (!_options.MergingRuels)
                        {
                            break;
                        }
                    }
                }
            }

            return result;
        }

        public virtual void Shutdown()
        {
            foreach (var workflowRulesResolver in _options.WorkflowRulesResolvers)
            {
                workflowRulesResolver.Shutdown();
            }
        }
    }
}
