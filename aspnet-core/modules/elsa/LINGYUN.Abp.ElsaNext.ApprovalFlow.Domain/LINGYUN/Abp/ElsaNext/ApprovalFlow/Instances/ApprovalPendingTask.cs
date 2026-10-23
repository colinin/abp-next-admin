using Elsa.UserTasks.Models;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;

public sealed record ApprovalPendingTask(ApprovalNodeTask NodeTask, UserTaskDetail Task);
