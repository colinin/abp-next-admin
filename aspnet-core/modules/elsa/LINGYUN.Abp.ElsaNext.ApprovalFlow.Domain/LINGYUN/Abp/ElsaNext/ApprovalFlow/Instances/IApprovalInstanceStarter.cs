using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;

public interface IApprovalInstanceStarter
{
    Task<ApprovalInstance> StartAsync(ApprovalInstanceStartRequest request, CancellationToken cancellationToken = default);
}
