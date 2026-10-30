using Elsa.Workflows.Runtime;
using Elsa.Workflows.Runtime.Requests;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;

public class ElsaApprovalWorkflowStarter : DomainService, IApprovalWorkflowStarter
{
	protected IWorkflowDispatcher Dispatcher { get; }

	public ElsaApprovalWorkflowStarter(IWorkflowDispatcher dispatcher)
	{
		Dispatcher = dispatcher;
	}

    public async Task<string> StartAsync(ApprovalWorkflowStartRequest request, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(request.DefinitionVersionId))
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Workflow.DefinitionVersionIdRequired);
		}
		if (string.IsNullOrWhiteSpace(request.InstanceId))
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Workflow.WorkflowInstanceIdRequired);
		}
		var dispatchRequest = new DispatchWorkflowDefinitionRequest(request.DefinitionVersionId)
		{
			InstanceId = request.InstanceId,
			CorrelationId = request.CorrelationId,
			Input = request.Input!.ToDictionary(x => x.Key, x => x.Value)!
        };
		await Dispatcher.DispatchAsync(dispatchRequest, new DispatchWorkflowOptions(), cancellationToken);
		return request.InstanceId;
	}
}
