using Elsa.ApprovalFlow;
using Elsa.ApprovalFlow.Models;
using Elsa.UserTasks.Contracts;
using Elsa.UserTasks.Models;
using Elsa.UserTasks.Services;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Decisions;

public class ApprovalActionResolver : DomainService, IApprovalActionResolver
{
	protected IApprovalNodeTaskRepository NodeTaskRepository { get; }

	protected IUserTaskManager UserTaskManager { get; }

	private static string CompleteOperationName => UserTaskModelMapper.ActionName(UserTaskAccessOperation.Complete);

	public ApprovalActionResolver(IApprovalNodeTaskRepository nodeTaskRepository, IUserTaskManager userTaskManager)
	{
		NodeTaskRepository = nodeTaskRepository;
		UserTaskManager = userTaskManager;
	}

	public async virtual Task<IReadOnlyList<ApprovalAvailableAction>> ResolveAsync(Guid instanceId, UserTaskActor actor, CancellationToken cancellationToken = default)
	{
		var actions = new Dictionary<string, ApprovalAvailableAction>(StringComparer.OrdinalIgnoreCase);
		foreach (var nodeTask in (await NodeTaskRepository.GetListByInstanceAsync(instanceId, cancellationToken)).Where(x => x.IsOpen()))
		{
			var task = await UserTaskManager.GetAsync(actor.Subject.TenantId, nodeTask.TaskId, actor, cancellationToken);
			if (task == null || !task.AllowedActions.Contains(CompleteOperationName, StringComparer.OrdinalIgnoreCase))
			{
				continue;
			}
			var effects = ApprovalTaskTags.ReadActionEffects(task.Tags);
			foreach (var action in task.Actions)
			{
				if (!actions.ContainsKey(action.Key))
				{
					var effect = (effects.TryGetValue(action.Key, out var resolved) ? resolved : ApprovalActionEffect.Neutral);
					actions[action.Key] = new ApprovalAvailableAction(action.Key, action.Label, effect);
				}
			}
		}
		return actions.Values.ToList();
	}
}
