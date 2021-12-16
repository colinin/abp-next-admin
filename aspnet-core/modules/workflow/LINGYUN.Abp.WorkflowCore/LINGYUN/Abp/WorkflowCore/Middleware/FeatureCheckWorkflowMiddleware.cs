using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Features;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.Middleware
{
    public class FeatureCheckWorkflowMiddleware : IWorkflowMiddleware
    {
        private readonly IFeatureChecker _featureChecker;
        private readonly ILogger<FeatureCheckWorkflowMiddleware> _logger;
        public WorkflowMiddlewarePhase Phase => WorkflowMiddlewarePhase.PreWorkflow;

        public FeatureCheckWorkflowMiddleware(
            IFeatureChecker featureChecker,
            ILogger<FeatureCheckWorkflowMiddleware> logger)
        {
            _featureChecker = featureChecker;
            _logger = logger;
        }

        public async Task HandleAsync(WorkflowInstance workflow, WorkflowDelegate next)
        {
            if (workflow.Data is IDictionary<string, object> dictionary &&
                dictionary.TryGetValue(WorkflowCoreConsts.FeatureField, out var defFeatures))
            {
                var requiresFeatures = defFeatures.ToString().Split(';');
                var passed = await _featureChecker.IsEnabledAsync(requiresAll: true, requiresFeatures);
                if (!passed)
                {
                    _logger.LogWarning("Workflow {0} was forcibly terminated for the following reasons: These required functions must be enabled: {1}",
                        workflow.Id,
                        requiresFeatures);
                    workflow.Status = WorkflowStatus.Terminated;
                }
            }

            await next();
        }
    }
}
