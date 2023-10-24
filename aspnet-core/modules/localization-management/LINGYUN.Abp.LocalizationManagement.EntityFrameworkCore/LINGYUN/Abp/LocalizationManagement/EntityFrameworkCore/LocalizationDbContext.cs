using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore
{
    [ConnectionStringName(LocalizationDbProperties.ConnectionStringName)]
    public class LocalizationDbContext : AbpDbContext<LocalizationDbContext>, ILocalizationDbContext
    {
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Text> Texts { get; set; }
        public LocalizationDbContext(
            DbContextOptions<LocalizationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureLocalization();
        }
    }
}
