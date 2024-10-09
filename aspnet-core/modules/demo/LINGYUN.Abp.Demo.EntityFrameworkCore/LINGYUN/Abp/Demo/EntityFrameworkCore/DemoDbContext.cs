using LINGYUN.Abp.DataProtection.EntityFrameworkCore;
using LINGYUN.Abp.Demo.Authors;
using LINGYUN.Abp.Demo.Books;
using Microsoft.EntityFrameworkCore;

namespace LINGYUN.Abp.Demo.EntityFrameworkCore;
public class DemoDbContext : AbpDataProtectionDbContext<DemoDbContext>
{
    public DbSet<Book> Books { get; set; }

    public DbSet<Author> Authors { get; set; }


    public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureDemo(); ;
    }
}
