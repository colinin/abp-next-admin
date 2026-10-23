using LINGYUN.Abp.ElsaNext.ApprovalFlow.Records;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Nodes;

public interface IApprovalNodeTransferHandler
{
    Task<ApprovalRecord> TransferAsync(ApprovalNodeTransferRequest request, CancellationToken cancellationToken = default);
}
