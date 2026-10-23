using LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;
using Elsa.UserTasks.Models;
using System;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;

public static class ParticipantColumns
{
	public const string DefaultProvider = ParticipantConsts.DefaultProvider;

	public static ParticipantReference? ToReference(string tenantId, string? provider, string? type, string? id, string? displayName)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			return null;
		}
		return new ParticipantReference(tenantId, string.IsNullOrWhiteSpace(provider) ? ParticipantConsts.DefaultProvider : provider, string.Equals(type, ParticipantConsts.GroupType, StringComparison.OrdinalIgnoreCase) ? UserTaskParticipantType.Group : UserTaskParticipantType.User, id, displayName);
	}

	public static bool Matches(string? provider, string? type, string? id, ParticipantReference other)
	{
		return !string.IsNullOrWhiteSpace(id) && string.Equals(id, other.Id, StringComparison.Ordinal) && 
            string.Equals(string.IsNullOrWhiteSpace(provider) ? ParticipantConsts.DefaultProvider : provider, other.Provider, StringComparison.Ordinal) && 
            object.Equals(string.Equals(type, ParticipantConsts.GroupType, StringComparison.OrdinalIgnoreCase) ? UserTaskParticipantType.Group : UserTaskParticipantType.User, other.Type);
	}
}
