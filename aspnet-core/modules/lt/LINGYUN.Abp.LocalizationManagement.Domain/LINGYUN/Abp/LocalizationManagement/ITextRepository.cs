using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.LocalizationManagement
{
    public interface ITextRepository : IRepository<Text, int>
    {
        Task<Text> GetByCultureKeyAsync(
            string resourceName,
            string cultureName,
            string key,
            CancellationToken cancellationToken = default
            );

        Task<List<Text>> GetListAsync(
            string resourceName,
            CancellationToken cancellationToken = default);

        Task<List<Text>> GetListAsync(
            string resourceName,
            string cultureName,
            CancellationToken cancellationToken = default);

        Task<int> GetDifferenceCountAsync(
            string cultureName,
            string targetCultureName,
            string resourceName = null,
            bool? onlyNull = null,
            string filter = null,
            CancellationToken cancellationToken = default);

        Task<List<TextDifference>> GetDifferencePagedListAsync(
            string cultureName,
            string targetCultureName,
            string resourceName = null,
            bool? onlyNull = null,
            string filter = null,
            string sorting = nameof(Text.Key),
            int skipCount = 1,
            int maxResultCount = 10,
            CancellationToken cancellationToken = default);
    }
}
