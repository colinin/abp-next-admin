using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Decisions;

public sealed record ApprovalDecisionResult(ApprovalRecord? Record, bool Ignored);
