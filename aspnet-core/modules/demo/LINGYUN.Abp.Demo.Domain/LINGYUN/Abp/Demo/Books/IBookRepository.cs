using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.Demo.Books;
public interface IBookRepository : IRepository<Book, Guid>
{
    Task<int> GetCountAsync(
        ISpecification<Book> specification,
        CancellationToken cancellationToken = default);

    Task<List<Book>> GetListAsync(
        ISpecification<Book> specification,
        string? sorting = nameof(Book.Name),
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
