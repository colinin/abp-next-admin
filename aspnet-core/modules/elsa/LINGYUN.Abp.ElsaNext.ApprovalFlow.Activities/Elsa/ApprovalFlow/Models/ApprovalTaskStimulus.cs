using Elsa.UserTasks.Models;
using System;
using System.Text.Json;

namespace Elsa.ApprovalFlow.Models;

public sealed record ApprovalTaskStimulus(string TaskId, string ActionKey, ParticipantReference? DecidedBy = null, JsonElement? Data = null, string? Comment = null, DateTimeOffset? DecidedAt = null);
