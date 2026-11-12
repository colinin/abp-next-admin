using LINGYUN.Abp.Demo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.Demo.Authors;
public class EfCoreAuthorRepository
    : EfCoreRepository<IDemoDbContext, Author, Guid>,
        IAuthorRepository
{
    public EfCoreAuthorRepository(
        IDbContextProvider<IDemoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<Author?> FindByNameAsync(string name)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(author => author.Name == name);
    }

    public async Task<List<Author>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string? sorting = nameof(Author.Name),
        string? filter = null)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet
            .WhereIf(
                !filter.IsNullOrWhiteSpace(),
                author => author.Name.Contains(filter!)
            )
            .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(Author.Name) : sorting)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync();
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<Author> specification,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<Author>> GetListAsync(
        ISpecification<Author> specification,
        string? sorting = nameof(Author.Name),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
              .Where(specification.ToExpression())
              .OrderBy(!sorting.IsNullOrWhiteSpace() ? sorting : nameof(Author.Name))
              .PageBy(skipCount, maxResultCount)
              .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
