using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.LocalizationManagement
{
    public interface ILanguageRepository : IRepository<Language, Guid>
    {
        Task<Language> FindByCultureNameAsync(
            string cultureName,
            CancellationToken cancellationToken = default);

        Task<List<Language>> GetActivedListAsync(CancellationToken cancellationToken = default);
    }
}
