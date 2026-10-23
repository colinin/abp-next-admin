using Elsa.Workflows.Management.Entities;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Workflows;

public interface IApprovalWorkflowCompiler
{
    Task<WorkflowDefinition> CompileAsync(ApprovalTemplate template, CancellationToken cancellationToken = default);
}
