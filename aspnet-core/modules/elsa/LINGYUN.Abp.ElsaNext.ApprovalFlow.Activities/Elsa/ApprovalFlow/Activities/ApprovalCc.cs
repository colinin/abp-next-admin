using Elsa.ApprovalFlow.Contracts;
using Elsa.ApprovalFlow.Models;
using Elsa.ApprovalFlow.Options;
using Elsa.Extensions;
using Elsa.UserTasks.Models;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Timing;

namespace Elsa.ApprovalFlow.Activities;

[Activity("Elsa", "Approval Flow", "Sends a carbon copy to the resolved recipients without waiting for them.")]
public sealed class ApprovalCc : CodeActivity
{
	[Input(Description = "The carbon copy title.")]
	public Input<string>? Title { get; set; } = null;

	[Input(Description = "The rules that resolve who receives the carbon copy.", UIHint = "json-editor")]
	public Input<IReadOnlyCollection<ApproverRule>?>? RecipientRules { get; set; } = null;

	[Input]
	public Input<string?>? Summary { get; set; } = null;

	[Input]
	public Input<string?>? Reference { get; set; } = null;

	[Input]
	public Input<ParticipantReference?>? Initiator { get; set; } = null;

	[Input]
	public Input<JsonElement?>? TaskData { get; set; } = null;

	protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
	{
        var approverResolver = context.GetRequiredService<IApproverResolver>();
        var options = context.GetService<IOptions<ApprovalFlowOptions>>()?.Value ?? new ApprovalFlowOptions();
		var workflow = context.WorkflowExecutionContext.Workflow;
		var tenantId = workflow.Identity.TenantId ?? options.DefaultTenantId;
        var rules = RecipientRules.GetOrDefault(context) ?? Array.Empty<ApproverRule>();
        // ExcludeInitiator belongs to approver rules; a carbon copy must include the initiator.
        foreach (var rule in rules)
        {
            rule.ExcludeInitiator = false;
        }
        var initiator = ResolveInitiator(context);
		var taskData = ResolveTaskData(context);
		var recipients = await approverResolver.ResolveAsync(
            new ApproverResolutionContext(tenantId, initiator, rules, taskData), 
            context.CancellationToken);
		if (recipients.Count != 0)
		{
            var approvalCcSink = context.GetRequiredService<IApprovalCcSink>();
            await approvalCcSink.NotifyAsync(
                new ApprovalCcRequest(tenantId, context.WorkflowExecutionContext.Id, ResolveTitle(context), 
                recipients, 
                new DateTimeOffset(context.GetRequiredService<IClock>().Now), 
                Summary.GetOrDefault(context), 
                ResolveReference(context), initiator, taskData),
                context.CancellationToken);
		}
	}

	public ApprovalCc()
		: base(null, null)
	{
	}

	private string ResolveTitle(ActivityExecutionContext context)
	{
		if (context.TryGetWorkflowInput<string>(ElsaNextApprovalFlowConsts.ApprovalTitleInputKey, out var title) &&
			!string.IsNullOrWhiteSpace(title))
		{
			return title;
		}

		return Title.Get(context, "Title");
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
}
