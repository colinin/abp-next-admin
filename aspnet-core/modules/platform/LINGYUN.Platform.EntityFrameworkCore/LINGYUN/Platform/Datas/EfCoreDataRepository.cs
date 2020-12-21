using LINGYUN.Platform.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Platform.Datas
{
    public class EfCoreDataRepository : EfCoreRepository<PlatformDbContext, Data, Guid>, IDataRepository
    {
        public EfCoreDataRepository(
            IDbContextProvider<PlatformDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<Data> FindByNameAsync(
            string name, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .Where(x => x.Name == name)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Data>> GetChildrenAsync(
            Guid? parentId, 
            bool includeDetails = false, 
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                  .IncludeDetails(includeDetails)
                  .Where(x => x.ParentId == parentId)
                  .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<int> GetCountAsync(
            string filter = "", 
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .WhereIf(!filter.IsNullOrWhiteSpace(), x =>
                    x.Code.Contains(filter) || x.Description.Contains(filter) ||
                    x.DisplayName.Contains(filter) || x.Name.Contains(filter))
                .CountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Data>> GetPagedListAsync(
            string filter = "", 
            string sotring = "Code", 
            bool includeDetails = false, 
            int skipCount = 0,
            int maxResultCount = 10, 
            CancellationToken cancellationToken = default)
        {
            sotring ??= nameof(Data.Code);

            return await DbSet
                .IncludeDetails(includeDetails)
                .WhereIf(!filter.IsNullOrWhiteSpace(), x =>
                      x.Code.Contains(filter) || x.Description.Contains(filter) ||
                      x.DisplayName.Contains(filter) || x.Name.Contains(filter))
                .OrderBy(sotring)
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public override IQueryable<Data> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }
    }
}
