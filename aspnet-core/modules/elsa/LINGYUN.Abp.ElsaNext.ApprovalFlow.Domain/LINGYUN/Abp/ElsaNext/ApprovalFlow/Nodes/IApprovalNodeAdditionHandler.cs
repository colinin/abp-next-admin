using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;

public interface IApprovalNodeAdditionHandler
{
	Task<ApprovalRecord> AddNodeAsync(ApprovalNodeAdditionRequest request, CancellationToken cancellationToken = default);

    Task<ApprovalRecord> RemoveNodeAsync(ApprovalNodeRemovalRequest request, CancellationToken cancellationToken = default);
}
