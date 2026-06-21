using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.PermissionManagement;

namespace LINGYUN.Abp.PermissionManagement;

public interface IPermissionGrantRepository : Volo.Abp.PermissionManagement.IPermissionGrantRepository
{
    Task<List<PermissionGrant>> GetListAsync(
        string[] names,
        string providerName,
        CancellationToken cancellationToken = default
    );
}
