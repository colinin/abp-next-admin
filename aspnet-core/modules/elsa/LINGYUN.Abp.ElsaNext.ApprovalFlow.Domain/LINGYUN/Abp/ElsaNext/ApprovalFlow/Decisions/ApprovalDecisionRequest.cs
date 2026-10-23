using Elsa.ApprovalFlow.Models;
using Elsa.UserTasks.Models;
using System;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Decisions;

public sealed record ApprovalDecisionRequest(Guid InstanceId, string TaskId, string ActionKey, ApprovalActionEffect Effect, UserTaskActor Actor, string OperationId, string? Comment = null, string? DataJson = null);
