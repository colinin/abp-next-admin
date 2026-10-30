using Elsa.ApprovalFlow.Contracts;
using Elsa.ApprovalFlow.Models;
using Elsa.ApprovalFlow.Options;
using Elsa.Expressions.Models;
using Elsa.Extensions;
using Elsa.UserTasks.Models;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Timing;

namespace Elsa.ApprovalFlow.Activities;

[Activity("Elsa", "Approval Flow", "Creates an approval node and waits for its approvers to decide.", Kind = ActivityKind.Task)]
public sealed class ApprovalTask : Activity<ApprovalTaskResult>
{
	public const string InputKey = "ApprovalTaskInput";

	public const string TaskTypeMarker = "approval";

	private const string StateProperty = "ApprovalNodeState";

	[Input(Description = "The node title shown on every approver's task.")]
	public Input<string>? Title { get; set; } = null;

	[Input]
	public Input<string?>? Summary { get; set; } = null;

	[Input]
	public Input<string?>? Reference { get; set; } = null;

	[Input]
	public Input<string?>? ApprovalNodeKey { get; set; } = null;

	[Input]
	public Input<string?>? ApprovalNodeName { get; set; } = null;

	[Input(Description = "The rules that resolve who may act on this node.", UIHint = "json-editor")]
	public Input<IReadOnlyCollection<ApproverRule>?>? ApproverRules { get; set; } = null;

	[Input(Description = "Any asks every approver at once and accepts the first decision; All asks everyone; Sequential asks one at a time.")]
	public Input<ApprovalSignMode> SignMode { get; set; } = new Input<ApprovalSignMode>(new Literal<ApprovalSignMode>(ApprovalSignMode.Any, null));

	[Input(Description = "The actions offered to every approver. Keys are literal; labels may use expressions.", DefaultSyntax = "Literal", SupportedSyntaxes = new string[] { "Literal" })]
	public Input<IReadOnlyCollection<ApprovalActionDefinition>?>? Actions { get; set; } = null;

	[Input]
	public Input<ParticipantReference?>? Initiator { get; set; } = null;

	[Input]
	public Input<string?>? Instructions { get; set; } = null;

	[Input]
	public Input<JsonElement?>? TaskData { get; set; } = null;

	[Input]
	public Input<UserTaskFormReference?>? Form { get; set; } = null;

	[Input]
	public Input<int> Priority { get; set; } = new Input<int>(new Literal<int>(50, null));

	[Input]
	public Input<DateTimeOffset?>? DueAt { get; set; } = null;

	[Input]
	public Input<TimeSpan?>? Timeout { get; set; } = null;

	[Input(Description = "What to do when the rules resolve to nobody. Skip passes the node without asking anybody.")]
	public Input<EmptyApproverPolicy> OnEmpty { get; set; } = new Input<EmptyApproverPolicy>(new Literal<EmptyApproverPolicy>(EmptyApproverPolicy.Skip, null));

	[Output(Description = "The node instance identifier, for correlation with the approval records.")]
	public Output<string>? NodeInstanceId { get; set; } = null;

	protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var approverResolver = context.GetRequiredService<IApproverResolver>();
		var options = context.GetService<IOptions<ApprovalFlowOptions>>()?.Value ?? new ApprovalFlowOptions();
		var workflow = context.WorkflowExecutionContext.Workflow;
		var tenantId = workflow.Identity.TenantId ?? options.DefaultTenantId;
        var rules = ApproverRules.GetOrDefault(context) ?? Array.Empty<ApproverRule>();
        var initiator = ResolveInitiator(context);
		var formData = ResolveTaskData(context);
		var approvers = await approverResolver.ResolveAsync(
            new ApproverResolutionContext(tenantId, initiator, rules, formData), 
            context.CancellationToken);
		if (approvers.Count == 0)
		{
			approvers = await ApplyEmptyApproverPolicyAsync(context, options, tenantId, initiator, rules, formData, context.CancellationToken);
			if (approvers.Count == 0)
			{
				return;
			}
		}
		if (approvers.Count > options.MaxApproversPerNode)
		{
			throw new BusinessException(ElsaNextApprovalFlowErrorCodes.ApprovalTask.TooManyApprovers);
		}
		var state = ApprovalNodeState.Create(context.Id, tenantId, SignMode.GetOrDefault(context), approvers);
		foreach (var action in Actions.GetOrDefault(context) ?? Array.Empty<ApprovalActionDefinition>())
		{
			if (action.Enabled)
			{
				state.ActionEffects[action.Key.ToString()] = action.Effect;
			}
		}
		context.SetProperty(StateProperty, state);
		NodeInstanceId.Set(context, context.Id, "NodeInstanceId");
		await DispatchAsync(context, state, options);
	}

	private async ValueTask<IReadOnlyCollection<ParticipantReference>> ApplyEmptyApproverPolicyAsync(ActivityExecutionContext context, ApprovalFlowOptions options, string tenantId, ParticipantReference? initiator, IReadOnlyCollection<ApproverRule> rules, JsonElement? formData, CancellationToken cancellationToken)
	{
		var policy = OnEmpty.GetOrDefault(context);
		if (policy == EmptyApproverPolicy.EscalateToManager)
        {
            var approverResolver = context.GetRequiredService<IApproverResolver>();
            var escalated = await approverResolver.ResolveAsync(
                new ApproverResolutionContext(
                    tenantId, 
                    initiator,
                    [new ApproverRule
                    {
                        Kind = ApproverKind.Supervisor
                    }], 
                    formData), 
                cancellationToken);
			if (escalated.Count > 0)
			{
				return escalated;
			}
		}
		if (policy == EmptyApproverPolicy.Skip)
		{
			context.WorkflowExecutionContext.Output[ElsaNextApprovalFlowConsts.OutcomeOutputKey] =
				ApprovalOutcome.Approved.ToString();
			await context.CompleteActivityAsync(new ApprovalTaskResult
			{
				ActionKey = "",
				IsApproved = true,
				Outcome = ApprovalOutcome.Approved,
				CompletedAt = GetNow(context)
			});
			return Array.Empty<ParticipantReference>();
		}
		var configured = string.Join(", ", rules.Select(x => x.Kind.ToString()).Distinct());
		throw new BusinessException(ElsaNextApprovalFlowErrorCodes.ApprovalTask.NoApprovers);
	}

	private ParticipantReference? ResolveInitiator(ActivityExecutionContext context)
	{
		var initiator = Initiator.GetOrDefault(context);
		if (initiator != null)
		{
			return initiator;
		}
		return context.TryGetWorkflowInput<ParticipantReference>(ElsaNextApprovalFlowConsts.InitiatorInputKey, out var fromWorkflow) ? fromWorkflow : null;
	}

	private string? ResolveReference(ActivityExecutionContext context)
	{
		var reference = Reference.GetOrDefault(context);
		if (!string.IsNullOrWhiteSpace(reference))
		{
			return reference;
		}
		return context.TryGetWorkflowInput<string>(ElsaNextApprovalFlowConsts.InstanceIdInputKey, out var instanceId) ? instanceId : null;
	}

	private UserTaskFormReference? ResolveForm(ActivityExecutionContext context)
	{
		var form = Form.GetOrDefault(context);
		if (form != null)
		{
			return form;
		}
		return context.TryGetWorkflowInput<UserTaskFormReference>(ElsaNextApprovalFlowConsts.ApprovalFormReferenceKey, out var formReference) ? formReference : null;
	}

	private JsonElement? ResolveTaskData(ActivityExecutionContext context)
	{
		var taskData = TaskData.GetOrDefault(context);
		if (taskData.HasValue)
		{
			return taskData;
		}
		if (context.TryGetWorkflowInput<string>(ElsaNextApprovalFlowConsts.FormDataInputKey, out var json) && !string.IsNullOrWhiteSpace(json))
		{
			using var document = JsonDocument.Parse(json);
			return document.RootElement.Clone();
		}
		return null;
	}
	private async ValueTask DispatchAsync(ActivityExecutionContext context, ApprovalNodeState state, ApprovalFlowOptions options)
	{
		if (state.SignMode == ApprovalSignMode.Any)
		{
			foreach (var slot in state.Slots)
			{
				slot.Dispatched = true;
			}

			state.ExpectedApproverCount = 1;
			context.SetProperty(StateProperty, state);

			await CreateBookmarkTaskAsync(context, state, null, state.Slots.Select(x => x.Approver).ToArray());
			return;
		}

		var pending = state.SignMode == ApprovalSignMode.Sequential
			? state.PendingSlots().Take(1).ToArray()
			: state.PendingSlots();

		foreach (var slot in pending)
		{
			slot.Dispatched = true;
		}

		state.ExpectedApproverCount = state.Slots.Count;
		context.SetProperty(StateProperty, state);

		foreach (var slot in pending)
		{
			await CreateBookmarkTaskAsync(context, state, slot.Approver, null);
		}
	}

	/// <summary>
	/// Creates a UserTask bookmark directly instead of scheduling a dynamic <c>UserTask</c> activity.
	/// Elsa 3.9 only allows scheduling activities that are part of the workflow graph, while approval
	/// approvers are resolved at runtime. The User Tasks projection pipeline materializes any bookmark
	/// whose stimulus is a <see cref="UserTaskMaterialization"/>, so creating the bookmark here produces
	/// the same persisted task as the built-in UserTask activity.
	/// </summary>
	private async ValueTask CreateBookmarkTaskAsync(
		ActivityExecutionContext context,
		ApprovalNodeState state,
		ParticipantReference? assignee,
		IReadOnlyCollection<ParticipantReference>? candidates)
	{
		var now = GetNow(context);
		var options = context.GetService<IOptions<ApprovalFlowOptions>>()?.Value ?? new ApprovalFlowOptions();
		var workflow = context.WorkflowExecutionContext.Workflow;
		var tenantId = workflow.Identity.TenantId ?? "";
		var bookmarkId = Guid.NewGuid().ToString("N");
		var taskId = Guid.NewGuid().ToString("N");
		var actions = BuildActions(context);
		var dueAt = DueAt.GetOrDefault(context);
		if (!dueAt.HasValue && options.DefaultTaskDueAfter.HasValue)
		{
			dueAt = now.Add(options.DefaultTaskDueAfter.Value);
		}

		var tags = new List<string>
		{
			TaskTypeMarker,
			ApprovalTaskTags.NodeInstance(state.NodeInstanceId),
			ApprovalTaskTags.SignMode(state.SignMode),
			ApprovalTaskTags.ActionEffects(actions),
			ApprovalTaskTags.Origin(ApprovalTaskTags.OriginWorkflow)
		};
		if (!string.IsNullOrWhiteSpace(ApprovalNodeKey.GetOrDefault(context)))
		{
			tags.Add(ApprovalTaskTags.NodeId(ApprovalNodeKey.GetOrDefault(context)!));
		}
		if (!string.IsNullOrWhiteSpace(ApprovalNodeName.GetOrDefault(context)))
		{
			tags.Add(ApprovalTaskTags.NodeName(ApprovalNodeName.GetOrDefault(context)!));
		}
		if (options.DiscloseApprovers && assignee != null)
		{
			tags.Add(ApprovalTaskTags.Assignee(assignee));
		}

		var definition = new UserTaskDefinitionSnapshot
		{
			Title = Title.Get(context, "Title"),
			Summary = Summary.GetOrDefault(context),
			Reference = ResolveReference(context),
			Tags = tags,
			TaskType = TaskTypeMarker,
			Requester = ResolveInitiator(context),
			Assignee = assignee,
			CandidateUsers = candidates ?? [],
			CandidateGroups = [],
			ExcludedUsers = [],
			MembershipResolutionMode = UserTaskMembershipResolutionMode.Live,
			Priority = Priority.GetOrDefault(context),
			DueAt = dueAt,
			Instructions = Instructions.GetOrDefault(context),
			TaskData = ResolveTaskData(context),
			FormReference = ResolveForm(context),
			Actions = actions.Select(action => new UserTaskAction(
				action.Key.ToString(),
				action.Label)).ToArray(),
			EnableTimeoutOutcome = Timeout.GetOrDefault(context).HasValue,
			EnableCancellationOutcome = true
		};

		var materialization = new UserTaskMaterialization(
			tenantId,
			workflow.Identity.DefinitionId,
			context.WorkflowExecutionContext.Id,
			context.Id,
			bookmarkId,
			definition,
			[],
			[],
			now,
			taskId,
			workflow.WorkflowMetadata.Name,
			workflow.Identity.Version,
			context.WorkflowExecutionContext.CorrelationId);

		context.CreateBookmark(new CreateBookmarkArgs
		{
			BookmarkId = bookmarkId,
			BookmarkName = nameof(Elsa.UserTasks.Activities.UserTask),
			Stimulus = materialization,
			Callback = OnTaskResumed,
			AutoComplete = false,
			IncludeActivityInstanceId = false
		});
	}

	private ApprovalActionDefinition[] BuildActions(ActivityExecutionContext context)
	{
		var actions = (Actions.GetOrDefault(context) ?? Array.Empty<ApprovalActionDefinition>())
			.Where(action => action.Enabled)
			.ToArray();

		if (actions.Length != 0)
		{
			return actions;
		}

		return
		[
			new ApprovalActionDefinition { Key = ApprovalActionKey.Approve, Effect = ApprovalActionEffect.Approve },
			new ApprovalActionDefinition { Key = ApprovalActionKey.Reject, Effect = ApprovalActionEffect.Reject }
		];
	}

	private async ValueTask OnTaskResumed(ActivityExecutionContext context)
	{
		var stimulus = context.GetWorkflowInput<UserTaskStimulus>(Elsa.UserTasks.Activities.UserTask.InputKey);
		var state = context.GetProperty<ApprovalNodeState>(StateProperty);
		if (state == null || state.Settled)
		{
			await context.CompleteActivityAsync();
			return;
		}

		var actionKey = stimulus.ActionKey ?? "";
		var approver = stimulus.CompletedBy ?? state.Slots.FirstOrDefault(slot => slot.Dispatched && !slot.Decided)?.Approver;
		if (approver == null || state.Slots.Count == 0)
		{
			await context.CompleteActivityAsync();
			return;
		}

		var effect = state.ActionEffects.TryGetValue(actionKey, out var mapped) ? mapped : ApprovalActionEffect.Approve;
		var decision = new ApproverDecision(
			approver,
			actionKey,
			effect,
			stimulus.CompletedAt ?? GetNow(context),
			stimulus.CompletionData,
			ReadComment(stimulus.CompletionData));
		state.Record(decision);
		context.SetProperty(StateProperty, state);

		var options = context.GetService<IOptions<ApprovalFlowOptions>>()?.Value ?? new ApprovalFlowOptions();
		var verdict = context.GetRequiredService<IApprovalCompletionPolicy>()
			.Evaluate(state.SignMode, state.Decisions, state.ExpectedApproverCount);

		if (verdict.IsFinal)
		{
			state.Settled = true;
			context.SetProperty(StateProperty, state);

			var result = BuildResult(context, state, verdict);
			context.WorkflowExecutionContext.Output[ElsaNextApprovalFlowConsts.OutcomeOutputKey] =
				result.Outcome?.ToString() ?? ApprovalOutcome.Approved.ToString();

			await context.CompleteActivityAsync(result);
			return;
		}

		if (state.SignMode == ApprovalSignMode.Sequential && state.PendingSlots().Count > 0)
		{
			await DispatchAsync(context, state, options);
		}
	}
	private static DateTimeOffset GetNow(ActivityExecutionContext context)
		=> new DateTimeOffset(context.GetRequiredService<IClock>().Now);

	private static ApprovalTaskResult BuildResult(ActivityExecutionContext context, ApprovalNodeState state, ApprovalPolicyResult verdict)
	{
		var approverDecision = state.Decisions.LastOrDefault();
		return new ApprovalTaskResult
		{
			ActionKey = verdict.ActionKey,
			IsApproved = verdict.IsApproved,
			Outcome = ((!verdict.IsApproved) ? ApprovalOutcome.Rejected : ApprovalOutcome.Approved),
			Data = approverDecision?.Data,
			Comment = approverDecision?.Comment,
			DecidedBy = approverDecision?.Approver,
			Decisions = state.Decisions.ToArray(),
			CompletedAt = (approverDecision?.DecidedAt ?? GetNow(context))
		};
	}

	private static string? ReadComment(JsonElement? data)
	{
		if (data.HasValue)
		{
			var valueOrDefault = data.GetValueOrDefault();
			if (valueOrDefault.ValueKind == JsonValueKind.Object && valueOrDefault.TryGetProperty("comment", out var value) && value.ValueKind == JsonValueKind.String)
			{
				return value.GetString();
			}
		}
		return null;
	}

	public ApprovalTask()
		: base(null, null)
	{
	}
}
