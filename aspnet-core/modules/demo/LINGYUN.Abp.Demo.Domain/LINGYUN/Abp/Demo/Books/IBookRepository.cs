using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Abp.Demo.Books;
public interface IBookRepository : IRepository<Book, Guid>
{
}
