using Elsa.UserTasks.Models;
using System;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;

public sealed record ApprovalNodeAdditionRequest(Guid InstanceId, ParticipantReference? Approver, UserTaskActor Actor, string? TaskId = null, string? Title = null, DateTimeOffset? DueAt = null, string? Comment = null);
