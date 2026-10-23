using Elsa.UserTasks.Models;
using System;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;

public sealed record ApprovalNodeRemovalRequest(Guid InstanceId, string NodeInstanceId, UserTaskActor Actor, string? Comment = null);
