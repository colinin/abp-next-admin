using Elsa.UserTasks.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace Elsa.ApprovalFlow.Models;

public sealed record ApproverResolutionContext(string TenantId, ParticipantReference? Initiator, IReadOnlyCollection<ApproverRule> Rules, JsonElement? FormData = null, IReadOnlyDictionary<string, object?>? Variables = null, IReadOnlyCollection<ParticipantReference>? PreSelectedParticipants = null);
