using Elsa.UserTasks.Contracts;
using Elsa.UserTasks.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;

public class ApprovalInstanceGuard : DomainService, IApprovalInstanceGuard
{
	protected IApprovalInstanceRepository InstanceRepository { get; }

	protected IApprovalNodeTaskRepository NodeTaskRepository { get; }

	protected IUserTaskManager UserTaskManager { get; }

	public ApprovalInstanceGuard(IApprovalInstanceRepository instanceRepository, IApprovalNodeTaskRepository nodeTaskRepository, IUserTaskManager userTaskManager)
	{
		InstanceRepository = instanceRepository;
		NodeTaskRepository = nodeTaskRepository;
		UserTaskManager = userTaskManager;
	}

    public virtual async Task<ApprovalInstance> GetRunningInstanceAsync(Guid instanceId, CancellationToken cancellationToken = default)
	{
		var instance = await InstanceRepository.GetAsync(instanceId, true, cancellationToken);
		if (instance.IsFinished())
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.Instance.AlreadyFinished);
		}
		return instance;
	}

	public virtual async Task<List<ApprovalPendingTask>> GetPendingTasksAsync(ApprovalInstance instance, UserTaskActor actor, CancellationToken cancellationToken = default)
	{
		Check.NotNull(instance, nameof(instance));
		Check.NotNull(actor, nameof(actor));
		var tenantId = ToUserTaskTenantId(instance);
		var pending = new List<ApprovalPendingTask>();
		foreach (var nodeTask in (await NodeTaskRepository.GetListByInstanceAsync(instance.Id, cancellationToken)).Where(x => x.IsOpen()))
		{
			var task = await UserTaskManager.GetAsync(tenantId, nodeTask.TaskId, actor, cancellationToken);
			if (task != null)
			{
				pending.Add(new ApprovalPendingTask(nodeTask, task));
			}
		}
		return pending;
	}

	public virtual async Task<bool> IsPendingApproverAsync(ApprovalInstance instance, UserTaskActor actor, CancellationToken cancellationToken = default)
	{
		return (await GetPendingTasksAsync(instance, actor, cancellationToken)).Count > 0;
	}

	public static string ToUserTaskTenantId(ApprovalInstance instance)
	{
		return instance.TenantId?.ToString("D") ?? "";
	}
}
