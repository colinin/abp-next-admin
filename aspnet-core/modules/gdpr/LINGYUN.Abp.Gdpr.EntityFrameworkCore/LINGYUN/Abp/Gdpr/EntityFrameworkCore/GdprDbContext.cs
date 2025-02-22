using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.Gdpr.EntityFrameworkCore;

[ConnectionStringName(GdprDbProterties.ConnectionStringName)]
public class GdprDbContext(DbContextOptions<GdprDbContext> options) : 
    AbpDbContext<GdprDbContext>(options), IGdprDbContext
{
    public virtual DbSet<GdprRequest> Requests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureGdpr(); ;
    }
}
