using Dapr.Client;
using System.Threading.Tasks;
using Volo.Abp.DistributedLocking;

namespace LINGYUN.Abp.DistributedLocking.Dapr;

public class DaprAbpDistributedLockHandle : IAbpDistributedLockHandle
{
    protected string StoreName { get; }
    protected string ResourceId { get; }
    protected string LockOwner { get; }
    protected DaprClient DaprClient { get; }
    public DaprAbpDistributedLockHandle(
        string storeName,
        string resourceId,
        string lockOwner,
        DaprClient daprClient)
    {
        StoreName = storeName;
        ResourceId = resourceId;
        LockOwner = lockOwner;
        DaprClient = daprClient;
    }
    public async ValueTask DisposeAsync()
    {
        await DaprClient.Unlock(StoreName, ResourceId, LockOwner);
    }
}
