using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Decisions;

public interface IApprovalDecisionHandler
{
	Task<ApprovalDecisionResult> DecideAsync(ApprovalDecisionRequest request, CancellationToken cancellationToken = default);
}
