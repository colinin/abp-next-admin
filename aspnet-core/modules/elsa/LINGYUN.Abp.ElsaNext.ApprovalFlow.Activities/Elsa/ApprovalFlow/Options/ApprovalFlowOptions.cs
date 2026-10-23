using System;

namespace Elsa.ApprovalFlow.Options;

public sealed class ApprovalFlowOptions
{
	public string DefaultTenantId { get; set; } = "";

	public int MaxApproversPerNode { get; set; } = 200;

	public bool RequireCommentOnReject { get; set; } = true;

	public TimeSpan? DefaultTaskDueAfter { get; set; }

	public bool DiscloseApprovers { get; set; } = true;
}
