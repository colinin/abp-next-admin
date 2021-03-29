using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore
{
    public class EfCoreTextRepository : EfCoreRepository<LocalizationDbContext, Text, int>,
        ITextRepository
    {
        public EfCoreTextRepository(
            IDbContextProvider<LocalizationDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<Text> GetByCultureKeyAsync(
            string resourceName,
            string cultureName,
            string key,
            CancellationToken cancellationToken = default
            )
        {
            return await (await GetDbSetAsync())
                .Where(x => x.ResourceName.Equals(resourceName) && x.CultureName.Equals(cultureName) && x.Key.Equals(key))
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<int> GetDifferenceCountAsync(
            string cultureName,
            string targetCultureName,
            string resourceName = null,
            bool? onlyNull = null,
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            return await (await BuildTextDifferenceQueryAsync(
                    cultureName,
                    targetCultureName,
                    resourceName,
                    onlyNull,
                    filter))
                .CountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Text>> GetListAsync(
            string resourceName, 
            CancellationToken cancellationToken = default)
        {
            var languages = (await GetDbContextAsync()).Set<Language>();
            var texts = await GetDbSetAsync();

            return await (from txts in texts
                   join lg in languages
                       on txts.CultureName equals lg.CultureName
                   where txts.ResourceName.Equals(resourceName) &&
                       lg.Enable
                   select txts)
                 .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Text>> GetListAsync(
            string resourceName,
            string cultureName, 
            CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                 .Where(x => x.ResourceName.Equals(resourceName) && x.CultureName.Equals(cultureName))
                 .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<TextDifference>> GetDifferencePagedListAsync(
            string cultureName,
            string targetCultureName,
            string resourceName = null,
            bool? onlyNull = null,
            string filter = null,
            string sorting = nameof(TextDifference.Key),
            int skipCount = 1,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default)
        {
            return await (await BuildTextDifferenceQueryAsync(
                    cultureName,
                    targetCultureName,
                    resourceName,
                    onlyNull,
                    filter,
                    sorting))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual async Task<IQueryable<TextDifference>> BuildTextDifferenceQueryAsync(
            string cultureName,
            string targetCultureName,
            string resourceName = null,
            bool? onlyNull = null,
            string filter = null,
            string sorting = nameof(TextDifference.Key))
        {
            var textQuery = (await GetDbSetAsync())
                .Where(x => x.CultureName.Equals(cultureName))
                .WhereIf(!resourceName.IsNullOrWhiteSpace(), x => x.ResourceName.Equals(resourceName))
                .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Key.Contains(filter))
                .OrderBy(sorting ?? nameof(TextDifference.Key));

            var targetTextQuery = (await GetDbSetAsync())
                .Where(x => x.CultureName.Equals(targetCultureName))
                .WhereIf(!resourceName.IsNullOrWhiteSpace(), x => x.ResourceName.Equals(resourceName));

            var query = from crtText in textQuery
                         join tgtText in targetTextQuery
                             on crtText.Key equals tgtText.Key
                             into tgt
                         from tt in tgt.DefaultIfEmpty()
                        where onlyNull.HasValue && onlyNull.Value 
                            ? tt.Value == null
                            : 1 == 1
                        select new TextDifference(
                             crtText.Id,
                             crtText.CultureName,
                             crtText.Key,
                             crtText.Value,
                             targetCultureName,
                             tt != null ? tt.Value : null,
                             crtText.ResourceName);
            return query;
        }
    }
}
