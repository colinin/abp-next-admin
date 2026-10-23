using System;
using System.Threading;
using System.Threading.Tasks;
using Elsa.ApprovalFlow.Models;
using Elsa.Mediator.Contracts;
using Elsa.UserTasks.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;
using Volo.Abp.Domain.Services;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;

public class UserTaskOverdueHandler : DomainService, INotificationHandler<UserTaskOverdue>, INotificationHandler
{
	protected IApprovalInstanceRepository InstanceRepository { get; }

	protected IApprovalNodeTaskRepository NodeTaskRepository { get; }

	protected IApprovalRecordRepository RecordRepository { get; }

	public UserTaskOverdueHandler(IApprovalInstanceRepository instanceRepository, IApprovalNodeTaskRepository nodeTaskRepository, IApprovalRecordRepository recordRepository)
	{
		InstanceRepository = instanceRepository;
		NodeTaskRepository = nodeTaskRepository;
		RecordRepository = recordRepository;
	}

	public async Task HandleAsync(UserTaskOverdue notification, CancellationToken cancellationToken)
	{
		Guid tenantId;
		using (CurrentTenant.Change(Guid.TryParse(notification.TenantId, out tenantId) ? new Guid?(tenantId) : ((Guid?)null)))
		{
			ApprovalNodeTask nodeTask = await NodeTaskRepository.FindByTaskIdAsync(notification.TaskId, cancellationToken);
			bool flag = nodeTask == null || !nodeTask.IsOpen();
			bool flag2 = flag;
			if (!flag2)
			{
				flag2 = (await InstanceRepository.FindAsync(nodeTask.InstanceId, includeDetails: true, cancellationToken))?.IsFinished() ?? true;
			}
			bool flag3 = flag2;
			if (!flag3)
			{
				flag3 = await RecordRepository.ExistsByActionAsync(nodeTask.InstanceId, nodeTask.TaskId, ApprovalRecordAction.TimeoutReminder.ToString(), cancellationToken);
			}
			if (flag3)
			{
				return;
			}
			ApprovalRecord record = new ApprovalRecord(GuidGenerator.Create(), nodeTask.InstanceId, ApprovalRecordAction.TimeoutReminder, ApprovalActionEffect.Neutral, new DateTimeOffset(Clock.Now), null, null, nodeTask.NodeId, nodeTask.NodeName, nodeTask.NodeInstanceId, nodeTask.TaskId);
			await RecordRepository.InsertAsync(record, autoSave: true, cancellationToken);
		}
	}
}
