using System.Threading;
using System.Threading.Tasks;

namespace Elsa.ApprovalFlow.Contracts;

public interface IApprovalCcSink
{
	Task NotifyAsync(ApprovalCcRequest request, CancellationToken cancellationToken = default);
}
