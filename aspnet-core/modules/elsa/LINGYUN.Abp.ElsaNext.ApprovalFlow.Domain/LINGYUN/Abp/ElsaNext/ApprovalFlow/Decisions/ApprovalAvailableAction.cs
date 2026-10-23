using Elsa.ApprovalFlow.Models;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Decisions;

public sealed record ApprovalAvailableAction(string Key, string Label, ApprovalActionEffect Effect);
