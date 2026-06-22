using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.LocalizationManagement;

public interface ILanguageRepository : IRepository<Language, Guid>
{
    Task<Language> FindByCultureNameAsync(
        string cultureName,
        CancellationToken cancellationToken = default);

    Task<List<Language>> GetActivedListAsync(
        CancellationToken cancellationToken = default);

    Task<int> GetCountAsync(
        ISpecification<Language> specification,
        CancellationToken cancellationToken = default);

    Task<List<Language>> GetListAsync(
        ISpecification<Language> specification,
        string sorting = nameof(Language.CultureName),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
