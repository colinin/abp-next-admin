using Elsa.UserTasks.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Elsa.ApprovalFlow.Models;

public sealed class ApprovalTaskResult
{
	public string ActionKey { get; set; } = "";

	public bool IsApproved { get; set; }

	public ApprovalOutcome? Outcome { get; set; }

	public JsonElement? Data { get; set; }

	public string? Comment { get; set; }

	public ParticipantReference? DecidedBy { get; set; }

	public IReadOnlyCollection<ApproverDecision> Decisions { get; set; } = Array.Empty<ApproverDecision>();

	public DateTimeOffset CompletedAt { get; set; }
}
