using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore;

public class EfCoreResourceRepository : EfCoreRepository<LocalizationDbContext, Resource, Guid>,
    IResourceRepository
{
    public EfCoreResourceRepository(
        IDbContextProvider<LocalizationDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async virtual Task<bool> ExistsAsync(
        string name, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).AnyAsync(x => x.Name.Equals(name));
    }

    [Obsolete("Use FindAsync() method.")]
    public virtual Resource FindByName(string name)
    {
        using (Volo.Abp.Uow.UnitOfWorkManager.DisableObsoleteDbContextCreationWarning.SetScoped(true))
        {
            return DbSet.FirstOrDefault(localizationResourceRecord => localizationResourceRecord.Name == name);
        }
    }

    public async virtual Task<Resource> FindByNameAsync(
        string name, 
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).Where(x => x.Name.Equals(name))
          .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }
}
