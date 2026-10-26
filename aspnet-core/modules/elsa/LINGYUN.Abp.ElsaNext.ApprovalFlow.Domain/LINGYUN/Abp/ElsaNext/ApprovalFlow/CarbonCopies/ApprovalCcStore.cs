using Elsa.ApprovalFlow.Contracts;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.CarbonCopies;

public class ApprovalCcStore : DomainService, IApprovalCcSink
{
	protected IApprovalCcRepository Repository { get; }

	protected IApprovalInstanceRepository InstanceRepository { get; }

	protected IApprovalNodeTaskRepository NodeTaskRepository { get; }

	public ApprovalCcStore(
		IApprovalCcRepository repository,
		IApprovalInstanceRepository instanceRepository,
		IApprovalNodeTaskRepository nodeTaskRepository)
	{
		Repository = repository;
		InstanceRepository = instanceRepository;
		NodeTaskRepository = nodeTaskRepository;
	}

    public async Task NotifyAsync(ApprovalCcRequest request, CancellationToken cancellationToken = default)
	{
		var instanceId = Guid.TryParse(request.Reference, out var parsed) ? new Guid?(parsed) : null;
		var instance = instanceId.HasValue
			? await InstanceRepository.FindAsync(instanceId.Value, cancellationToken: cancellationToken)
			: await InstanceRepository.FindByElsaWorkflowInstanceIdAsync(request.NodeInstanceId, cancellationToken: cancellationToken);

		if (instance != null)
		{
			instanceId = instance.Id;

			// A rejected/withdrawn/cancelled approval must not notify CC recipients even
			// when the workflow continuation still reaches the CC node.
			if (instance.Status is ApprovalInstanceStatus.Rejected
				or ApprovalInstanceStatus.Cancelled
				or ApprovalInstanceStatus.Withdrawn
				or ApprovalInstanceStatus.Returned)
			{
				return;
			}

			// Older workflow definitions may still execute the CC node before the approval
			// branch. Require at least one decided approval node so CC is never sent early.
			var nodeTasks = await NodeTaskRepository.GetListByInstanceAsync(instance.Id, cancellationToken: cancellationToken);
			if (nodeTasks.All(x => x.Status != ApprovalNodeTaskStatus.Decided))
			{
				return;
			}
		}

		var title = instance?.Title ?? request.Title;
		var instanceReference = instance?.Id.ToString("D") ?? request.Reference;

		using var enumerator = request.Recipients.GetEnumerator();
		while (enumerator.MoveNext())
		{
			var cc = new ApprovalCc(
                recipient: enumerator.Current,
                id: GuidGenerator.Create(), 
                title: title, 
                notifiedAt: request.NotifiedAt, 
                instanceId: instanceId, 
                instanceReference: instanceReference, 
                nodeId: null, 
                nodeInstanceId: request.NodeInstanceId, 
                summary: request.Summary, 
                sender: request.Sender,
                dataJson: request.TaskData?.ToString());
			await Repository.InsertAsync(cc, true, cancellationToken);
		}
	}
}
