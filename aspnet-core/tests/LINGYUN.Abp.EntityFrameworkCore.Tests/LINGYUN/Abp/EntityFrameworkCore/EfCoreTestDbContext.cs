using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Abp.EntityFrameworkCore;
public class EfCoreTestDbContext : AbpDbContext<EfCoreTestDbContext>
{
    public virtual DbSet<EfCoreTestEntity> TestEntities { get; set; }

    public EfCoreTestDbContext(
        DbContextOptions<EfCoreTestDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EfCoreTestEntity>(b =>
        {
            b.ConfigureByConvention();
        });
    }
}
