using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.DataProtectionManagement;
public interface IProtectedEntitiesSaver
{
    Task SaveAsync(CancellationToken cancellationToken = default);
}
