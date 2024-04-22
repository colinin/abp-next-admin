using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace LINGYUN.Abp.DistributedLocking.Dapr;

public class LockOwnerFinder : ILockOwnerFinder, ITransientDependency
{
    protected ICurrentUser CurrentUser { get; }
    protected AbpDistributedLockingDaprOptions Options { get; }

    public LockOwnerFinder(
        ICurrentUser currentUser,
        IOptions<AbpDistributedLockingDaprOptions> options)
    {
        Options = options.Value;
        CurrentUser = currentUser;
    }

    public Task<string> FindAsync()
    {
        if (CurrentUser.IsAuthenticated)
        {
            return Task.FromResult(CurrentUser.GetId().ToString("N"));
        }

        return Task.FromResult(Options.DefaultIdentifier);
    }
}
