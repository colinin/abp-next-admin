using LINGYUN.Abp.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Abp.DataProtection;
public class AbpDataProtectionTestDbContext : AbpDataProtectionDbContext<AbpDataProtectionTestDbContext>
{
    public DbSet<FakeProtectionObject> FakeProtectionObjects { get; set; }
    public AbpDataProtectionTestDbContext(
        DbContextOptions<AbpDataProtectionTestDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<FakeProtectionObject>(b =>
        {
            b.ConfigureByConvention();
        });
    }
}
