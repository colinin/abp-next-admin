using System.Threading.Tasks;

namespace LINGYUN.Abp.DistributedLocking.Dapr;

public interface ILockOwnerFinder
{
    Task<string> FindAsync();
}
