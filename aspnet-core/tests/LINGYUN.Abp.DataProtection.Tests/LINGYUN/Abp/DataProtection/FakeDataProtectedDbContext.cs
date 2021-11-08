using LINGYUN.Abp.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LINGYUN.Abp.DataProtection
{
    public class FakeDataProtectedDbContext : AbpDataProtectionDbContext<FakeDataProtectedDbContext>
    {
        public FakeDataProtectedDbContext(
            DbContextOptions<FakeDataProtectedDbContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FakeProtectionObject>(b =>
            {
                b.Property(p => p.Owner).HasColumnName(nameof(IDataProtection.Owner)).HasMaxLength(200);
            });
        }
    }
}
