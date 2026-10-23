using Elsa.UserTasks.Models;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Actors;

public interface IApprovalActorAccessor
{
    Task<UserTaskActor?> GetAsync(CancellationToken cancellationToken = default);
}
