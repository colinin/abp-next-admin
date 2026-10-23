using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;

public interface IApprovalWorkflowStarter
{
    Task<string> StartAsync(ApprovalWorkflowStartRequest request, CancellationToken cancellationToken = default);
}
