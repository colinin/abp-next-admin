using Elsa.UserTasks.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Elsa.ApprovalFlow.Contracts;

public sealed record ApprovalCcRequest(string TenantId, string NodeInstanceId, string Title, IReadOnlyCollection<ParticipantReference> Recipients, DateTimeOffset NotifiedAt, string? Summary = null, string? Reference = null, ParticipantReference? Sender = null, JsonElement? TaskData = null);
