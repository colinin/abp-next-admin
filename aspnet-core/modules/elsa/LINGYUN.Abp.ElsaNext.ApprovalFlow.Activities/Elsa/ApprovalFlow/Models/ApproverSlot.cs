using Elsa.UserTasks.Models;

namespace Elsa.ApprovalFlow.Models;

public sealed class ApproverSlot
{
	public ParticipantReference Approver { get; set; } = new ParticipantReference("", "", UserTaskParticipantType.User, "");

	public bool Dispatched { get; set; }

	public bool Decided { get; set; }
}
