using Elsa.UserTasks.Models;
using System;
using System.Text.Json;

namespace Elsa.ApprovalFlow.Models;

public sealed record ApproverDecision(ParticipantReference Approver, string ActionKey, ApprovalActionEffect Effect, DateTimeOffset DecidedAt, JsonElement? Data = null, string? Comment = null);
