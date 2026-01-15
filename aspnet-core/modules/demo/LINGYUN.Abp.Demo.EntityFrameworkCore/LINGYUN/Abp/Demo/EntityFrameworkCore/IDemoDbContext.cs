using LINGYUN.Abp.Demo.Authors;
using LINGYUN.Abp.Demo.Books;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.Demo.EntityFrameworkCore;

[ConnectionStringName(DemoDbProterties.ConnectionStringName)]
public interface IDemoDbContext : IEfCoreDbContext
{
    DbSet<Book> Books { get; }

    DbSet<Author> Authors { get; }
}
