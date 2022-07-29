using Elsa.Models;
using Elsa.Services;
using Elsa.Services.Models;
using Elsa.Services.Workflows;
using Elsa.Services.WorkflowStorage;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.Elsa;

public class AbpWorkflowRunner : WorkflowRunner
{
    private readonly ICurrentTenant _currentTenant;
    public AbpWorkflowRunner(
        ICurrentTenant currentTenant,
        IWorkflowContextManager workflowContextManager, 
        IMediator mediator, 
        IServiceScopeFactory serviceScopeFactory, 
        IGetsStartActivities startingActivitiesProvider, 
        IWorkflowStorageService workflowStorageService, 
        ILogger<WorkflowRunner> logger) 
        : base(
            workflowContextManager, 
            mediator, 
            serviceScopeFactory, 
            startingActivitiesProvider, 
            workflowStorageService, 
            logger)
    {
        _currentTenant = currentTenant;
    }

    protected async override Task<RunWorkflowResult> RunWorkflowInternalAsync(
        WorkflowExecutionContext workflowExecutionContext,
        string? activityId = null, 
        CancellationToken cancellationToken = default)
    {
        var tenantId = workflowExecutionContext.WorkflowInstance.GetTenantId();

        using (_currentTenant.Change(tenantId))
        {
            return await base.RunWorkflowInternalAsync(
                workflowExecutionContext,
                activityId,
                cancellationToken);
        }
    }
}
