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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LINGYUN.Platform.Datas
{
    public class EfCoreDataRepository : EfCoreRepository<PlatformDbContext, Data, Guid>, IDataRepository
    {
        public EfCoreDataRepository(
            IDbContextProvider<PlatformDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async virtual Task<Data> FindByNameAsync(
            string name, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .IncludeDetails(includeDetails)
                .Where(x => x.Name == name)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public async virtual Task<List<Data>> GetChildrenAsync(
            Guid? parentId, 
            bool includeDetails = false, 
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                  .IncludeDetails(includeDetails)
                  .Where(x => x.ParentId == parentId)
                  .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async virtual Task<int> GetCountAsync(
            string filter = "", 
            CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet
                .WhereIf(!filter.IsNullOrWhiteSpace(), x =>
                    x.Code.Contains(filter) || x.Description.Contains(filter) ||
                    x.DisplayName.Contains(filter) || x.Name.Contains(filter))
                .CountAsync(GetCancellationToken(cancellationToken));
        }

        public async virtual Task<List<Data>> GetPagedListAsync(
            string filter = "", 
            string sorting = "Code", 
            bool includeDetails = false, 
            int skipCount = 0,
            int maxResultCount = 10, 
            CancellationToken cancellationToken = default)
        {
            if (sorting.IsNullOrWhiteSpace())
            {
                sorting = nameof(Data.Code);
            }

            var dbSet = await GetDbSetAsync();
            return await dbSet
                .IncludeDetails(includeDetails)
                .WhereIf(!filter.IsNullOrWhiteSpace(), x =>
                      x.Code.Contains(filter) || x.Description.Contains(filter) ||
                      x.DisplayName.Contains(filter) || x.Name.Contains(filter))
                .OrderBy(sorting)
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public override async Task<IQueryable<Data>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).IncludeDetails();
        }

        [System.Obsolete("将在abp框架移除之后删除")]
        public override IQueryable<Data> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }
    }
}
