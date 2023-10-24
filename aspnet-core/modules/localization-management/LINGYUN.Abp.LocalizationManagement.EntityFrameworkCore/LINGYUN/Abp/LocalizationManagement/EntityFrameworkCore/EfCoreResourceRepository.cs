using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore
{
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

        public async virtual Task<Resource> FindByNameAsync(
            string name, 
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync()).Where(x => x.Name.Equals(name))
              .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }
    }
}
