using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    public class WorkflowsResolver : IWorkflowsResolver, ITransientDependency
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly AbpRulesEngineResolveOptions _options;

        public WorkflowsResolver(
            IServiceScopeFactory serviceScopeFactory,
            IOptions<AbpRulesEngineResolveOptions> options)
        {
            _options = options.Value;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public virtual void Initialize(RulesInitializationContext context)
        {
            foreach (var workflowRulesResolver in _options.WorkflowsResolvers)
            {
                workflowRulesResolver.Initialize(context);
            }
        }

        public async virtual Task<WorkflowsResolveResult> ResolveWorkflowsAsync(Type type)
        {
            var result = new WorkflowsResolveResult();

            using (var serviceScope = _serviceScopeFactory.CreateScope())
            {
                var context = new WorkflowsResolveContext(type, serviceScope.ServiceProvider);

                foreach (var workflowRulesResolver in _options.WorkflowsResolvers)
                {
                    await workflowRulesResolver.ResolveAsync(context);

                    result.AppliedResolvers.Add(workflowRulesResolver.Name);

                    if (context.HasResolved())
                    {
                        result.Workflows.AddRange(context.Workflows);

                        if (!_options.MergingWorkflows)
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
            foreach (var workflowRulesResolver in _options.WorkflowsResolvers)
            {
                workflowRulesResolver.Shutdown();
            }
        }
    }
}
