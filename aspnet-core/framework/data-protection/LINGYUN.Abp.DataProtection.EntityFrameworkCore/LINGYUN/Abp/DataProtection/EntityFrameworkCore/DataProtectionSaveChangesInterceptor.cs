using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.DataProtection.EntityFrameworkCore;
public class DataProtectionSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly static EntityState[] _protectedStates =
    [
        EntityState.Modified,
        EntityState.Detached,
    ];
    public async override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result, 
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context != null && eventData.Context is IAbpEfCoreDbContext)
        {
            // 存在资源所属者的实体将被检查访问权限
            var changeEntires = eventData.Context.ChangeTracker.Entries<IMayHaveCreator>()
                .Where(e => _protectedStates.Contains(e.State))
                .Where(e => e.Entity.CreatorId.HasValue);

            foreach (var changeEntity in changeEntires)
            {

            }
        }
        // return InterceptionResult<int>.SuppressWithResult(0);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
