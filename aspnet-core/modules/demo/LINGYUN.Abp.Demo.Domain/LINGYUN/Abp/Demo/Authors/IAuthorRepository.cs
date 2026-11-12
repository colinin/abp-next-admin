using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.Demo.Authors;
public interface IAuthorRepository : IRepository<Author, Guid>
{
    Task<Author?> FindByNameAsync(string name);

    Task<List<Author>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string? sorting = nameof(Author.Name),
        string? filter = null
    );

    Task<int> GetCountAsync(
        ISpecification<Author> specification,
        CancellationToken cancellationToken = default);

    Task<List<Author>> GetListAsync(
        ISpecification<Author> specification,
        string? sorting = nameof(Author.Name),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
