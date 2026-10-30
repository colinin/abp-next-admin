using Elsa.ApprovalFlow.Activities;
using System;
using System.Threading;
using System.Threading.Tasks;
using Elsa.ApprovalFlow;
using Elsa.Mediator.Contracts;
using Elsa.UserTasks.Contracts;
using Elsa.UserTasks.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;
using Volo.Abp.Domain.Services;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Handlers;

public class UserTaskCreatedHandler : DomainService, INotificationHandler<UserTaskCreated>, INotificationHandler
{
	protected IUserTaskRepository UserTaskRepository { get; }

	protected IApprovalNodeTaskRepository NodeTaskRepository { get; }

	public UserTaskCreatedHandler(IUserTaskRepository userTaskRepository, IApprovalNodeTaskRepository nodeTaskRepository)
	{
		UserTaskRepository = userTaskRepository;
		NodeTaskRepository = nodeTaskRepository;
	}

	public async Task HandleAsync(UserTaskCreated notification, CancellationToken cancellationToken)
	{
		UserTask task = await UserTaskRepository.GetAsync(notification.TenantId, notification.TaskId, cancellationToken);
		if (task != null && string.Equals(task.TaskType, ApprovalTask.TaskTypeMarker, StringComparison.Ordinal))
		{
			string nodeInstanceId = ApprovalTaskTags.ReadNodeInstance(task.Tags);
			Guid instanceId = default;
			bool flag = nodeInstanceId != null && Guid.TryParse(task.Reference, out instanceId);
			bool flag2 = flag;
			if (flag2)
			{
				flag2 = await NodeTaskRepository.FindByTaskIdAsync(task.Id, cancellationToken) == null;
			}
			if (flag2)
			{
				ApprovalNodeTask nodeTask = new ApprovalNodeTask(signMode: ApprovalTaskTags.ReadSignMode(task.Tags).GetValueOrDefault(), origin: string.Equals(ApprovalTaskTags.ReadOrigin(task.Tags), ApprovalTaskTags.OriginAdded, StringComparison.Ordinal) ? ApprovalNodeTaskOrigin.Added : ApprovalNodeTaskOrigin.Workflow, id: GuidGenerator.Create(), instanceId: instanceId, nodeInstanceId: nodeInstanceId, taskId: task.Id, approver: task.Assignee, nodeId: ApprovalTaskTags.ReadNodeId(task.Tags), nodeName: ApprovalTaskTags.ReadNodeName(task.Tags));
				await NodeTaskRepository.InsertAsync(nodeTask, autoSave: true, cancellationToken);
			}
		}
	}
}
