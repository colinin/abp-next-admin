using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace LINGYUN.BackendAdmin.EntityFrameworkCore
{
    public class BackendAdminHostMigrationsDbContext : AbpDbContext<BackendAdminHostMigrationsDbContext>
    {
        public BackendAdminHostMigrationsDbContext(DbContextOptions<BackendAdminHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.UseMySQL();
            modelBuilder.ConfigureIdentity();
            modelBuilder.ConfigureIdentityServer();
            //modelBuilder.ConfigureTenantManagement();
            //modelBuilder.ConfigureSettingManagement();
            //modelBuilder.ConfigurePermissionManagement();
        }
    }
}
