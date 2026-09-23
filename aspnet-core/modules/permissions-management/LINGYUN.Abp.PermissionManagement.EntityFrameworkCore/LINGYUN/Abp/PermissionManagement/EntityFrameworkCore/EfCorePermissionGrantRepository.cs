using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;

namespace LINGYUN.Abp.PermissionManagement.EntityFrameworkCore;

public class EfCorePermissionGrantRepository : Volo.Abp.PermissionManagement.EntityFrameworkCore.EfCorePermissionGrantRepository, IPermissionGrantRepository
{
    public EfCorePermissionGrantRepository(
        IDbContextProvider<IPermissionManagementDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async virtual Task<List<Volo.Abp.PermissionManagement.PermissionGrant>> GetListAsync(
        string[] names, 
        string providerName, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(s =>
                names.Contains(s.Name) &&
                s.ProviderName == providerName
            ).ToListAsync(GetCancellationToken(cancellationToken));
    }
}
