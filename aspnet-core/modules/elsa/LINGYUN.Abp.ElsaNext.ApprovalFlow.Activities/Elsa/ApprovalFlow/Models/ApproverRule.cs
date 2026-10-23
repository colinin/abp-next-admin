using Elsa.UserTasks.Models;
using System.Collections.Generic;

namespace Elsa.ApprovalFlow.Models;

public sealed class ApproverRule
{
	public ApproverKind Kind { get; set; } = ApproverKind.Participant;

	public ICollection<ParticipantReference> Participants { get; set; } = new List<ParticipantReference>();

	public SupervisorLevelKind SupervisorLevel { get; set; } = SupervisorLevelKind.Direct;

	public int Level { get; set; } = 1;

	public string? FormFieldKey { get; set; }

	public bool ExcludeInitiator { get; set; } = true;

	public bool Deduplicate { get; set; } = true;

	public EmptyApproverPolicy OnEmpty { get; set; } = EmptyApproverPolicy.Skip;
}
