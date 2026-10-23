using Elsa.UserTasks.Models;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Delegations;

public interface IApprovalDelegationResolver
{
    Task<bool> IsDelegatedAsync(ParticipantReference delegator, ParticipantReference actor, CancellationToken cancellationToken = default);

    Task<bool> HasActiveDelegateAsync(ParticipantReference delegator, CancellationToken cancellationToken = default);
}
