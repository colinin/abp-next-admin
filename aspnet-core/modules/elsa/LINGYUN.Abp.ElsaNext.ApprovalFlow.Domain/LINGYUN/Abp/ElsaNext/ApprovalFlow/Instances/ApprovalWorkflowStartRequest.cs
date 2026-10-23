using System.Collections.Generic;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;

public sealed record ApprovalWorkflowStartRequest(string DefinitionVersionId, string InstanceId, string CorrelationId, IReadOnlyDictionary<string, object?> Input);
