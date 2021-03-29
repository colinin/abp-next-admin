using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore
{
    public class LocalizationManagementHttpApiHostMigrationsDbContext : AbpDbContext<LocalizationManagementHttpApiHostMigrationsDbContext>
    {
        public LocalizationManagementHttpApiHostMigrationsDbContext(
            DbContextOptions<LocalizationManagementHttpApiHostMigrationsDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureLocalization();
        }
    }
}
