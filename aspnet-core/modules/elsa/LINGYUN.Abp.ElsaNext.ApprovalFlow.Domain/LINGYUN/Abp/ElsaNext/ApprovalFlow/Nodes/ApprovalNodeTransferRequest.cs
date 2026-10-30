using Elsa.UserTasks.Models;
using System;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;

public sealed record ApprovalNodeTransferRequest(Guid InstanceId, ParticipantReference? Transferee, UserTaskActor Actor, string? TaskId = null, string? Comment = null);
