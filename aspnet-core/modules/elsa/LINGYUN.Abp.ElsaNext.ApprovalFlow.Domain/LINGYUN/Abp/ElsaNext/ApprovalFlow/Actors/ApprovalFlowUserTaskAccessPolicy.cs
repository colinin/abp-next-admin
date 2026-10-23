using Elsa.ApprovalFlow.Activities;
using Elsa.UserTasks.Contracts;
using Elsa.UserTasks.Models;
using Elsa.UserTasks.Permissions;
using Elsa.UserTasks.Services;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Delegations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Actors;

public class ApprovalFlowUserTaskAccessPolicy : IUserTaskAccessPolicy
{
	private static readonly UserTaskAccessOperation[] DelegatableOperations = new UserTaskAccessOperation[3]
	{
		UserTaskAccessOperation.ReadSummary,
		UserTaskAccessOperation.ReadProtected,
		UserTaskAccessOperation.Complete
	};

	protected DefaultUserTaskAccessPolicy Inner { get; }

	protected IApprovalDelegationResolver DelegationResolver { get; }

	public ApprovalFlowUserTaskAccessPolicy(IApprovalDelegationResolver delegationResolver)
	{
		DelegationResolver = delegationResolver;
		Inner = new DefaultUserTaskAccessPolicy();
	}

	public virtual Task<UserTaskQueryScope?> CreateScopeAsync(UserTaskActor actor, UserTaskQueryScopeKind kind, CancellationToken cancellationToken = default)
	{
		if (kind == UserTaskQueryScopeKind.All)
		{
			return Task.FromResult<UserTaskQueryScope?>(
				new UserTaskQueryScope(
					actor.Subject.TenantId,
					actor.Subject,
					actor.Groups,
					actor.IsManager,
					kind,
					ExcludeBlocking: true));
		}

		return Inner.CreateScopeAsync(actor, kind, cancellationToken);
	}

    public virtual async Task<bool> AuthorizeAsync(UserTask task, UserTaskActor actor, UserTaskAccessOperation operation, CancellationToken cancellationToken = default)
	{
		if (await Inner.AuthorizeAsync(task, actor, operation, cancellationToken))
		{
			return true;
		}
		if (actor.IsGuest)
		{
			return false;
		}
		if (task.IsTerminal)
		{
			return false;
		}
		if (!string.Equals(task.TenantId, actor.Subject.TenantId, StringComparison.Ordinal))
		{
			return false;
		}

		if (DelegatableOperations.Contains(operation, null) &&
            await IsDelegatedAsync(task, actor, operation, cancellationToken))
		{
			return true;
		}
		return IsApprovalTaskTransfer(task, actor, operation);
	}

	protected virtual async Task<bool> IsDelegatedAsync(UserTask task, UserTaskActor actor, UserTaskAccessOperation operation, CancellationToken cancellationToken = default)
	{
		if (task.Assignee == null || task.Assignee.Matches(actor.Subject))
		{
			return false;
		}
		if (!(await DelegationResolver.IsDelegatedAsync(task.Assignee, actor.Subject, cancellationToken)))
		{
			return false;
		}
		var delegatorActor = new UserTaskActor(task.Assignee, Array.Empty<ParticipantReference>(), task.Assignee.DisplayName)
		{
			IsManager = actor.IsManager,
			Permissions = actor.Permissions
		};
		return await Inner.AuthorizeAsync(task, delegatorActor, operation, cancellationToken);
	}

	protected virtual bool IsApprovalTaskTransfer(UserTask task, UserTaskActor actor, UserTaskAccessOperation operation)
	{
		if (operation == UserTaskAccessOperation.Assign && string.Equals(task.TaskType, ApprovalTask.TaskTypeMarker, StringComparison.Ordinal))
		{
			var assignee = task.Assignee;
			if (assignee != null && assignee.Matches(actor.Subject))
			{
				return (actor.HasPermission(UserTasksResourcePermissions.UserTasks, UserTaskVerbs.Assign) ? 1 : 0) != 0;
			}
		}
		return true;
	}
}
