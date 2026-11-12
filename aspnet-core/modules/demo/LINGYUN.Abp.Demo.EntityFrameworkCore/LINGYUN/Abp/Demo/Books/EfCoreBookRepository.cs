using JetBrains.Annotations;
using LINGYUN.Abp.DataProtection;
using LINGYUN.Abp.DataProtection.EntityFrameworkCore;
using LINGYUN.Abp.Demo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.Demo.Books;
public class EfCoreBookRepository : EfCoreDataProtectionRepository<DemoDbContext, Book, Guid, BookAuth>, IBookRepository
{
    public EfCoreBookRepository(
        [NotNull] IDbContextProvider<DemoDbContext> dbContextProvider,
        [NotNull] IDataAuthorizationService dataAuthorizationService,
        [NotNull] IEntityTypeFilterBuilder entityTypeFilterBuilder,
        [NotNull] IEntityPropertyResultBuilder entityPropertyResultBuilder) 
        : base(dbContextProvider, dataAuthorizationService, entityTypeFilterBuilder, entityPropertyResultBuilder)
    {
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<Book> specification,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<Book>> GetListAsync(
        ISpecification<Book> specification,
        string? sorting = nameof(Book.Name),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
              .Where(specification.ToExpression())
              .OrderBy(!sorting.IsNullOrWhiteSpace() ? sorting : nameof(Book.Name))
              .PageBy(skipCount, maxResultCount)
              .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
