using Elsa.UserTasks.Models;
using System;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Participants;

public static class ParticipantReferenceFactory
{
    public const string UserProvider = "identity";

    public const string GroupProvider = "organization-unit";

    public const string RoleProvider = "role";

    public static ParticipantReference User(string tenantId, Guid userId, string? displayName = null, string? provider = null)
    {
        return new ParticipantReference(tenantId, provider ?? UserProvider, UserTaskParticipantType.User, userId.ToString("D"), displayName);
    }

    public static ParticipantReference User(string tenantId, string userId, string? displayName = null, string? provider = null)
    {
        return new ParticipantReference(tenantId, provider ?? UserProvider, UserTaskParticipantType.User, userId, displayName);
    }

    public static ParticipantReference Group(string tenantId, Guid organizationUnitId, string? displayName = null)
    {
        return new ParticipantReference(tenantId, GroupProvider, UserTaskParticipantType.Group, organizationUnitId.ToString("D"), displayName);
    }

    public static ParticipantReference Role(string tenantId, string roleName, string? displayName = null)
    {
        return new ParticipantReference(tenantId, RoleProvider, UserTaskParticipantType.Group, roleName, displayName ?? roleName);
    }

    public static Guid? TryGetId(ParticipantReference reference)
    {
        Guid result;
        return Guid.TryParse(reference.Id, out result) ? new Guid?(result) : ((Guid?)null);
    }
}
