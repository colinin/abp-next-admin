using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace LINGYUN.ApiGateway.EntityFrameworkCore
{
    public class HttpApiHostMigrationsDbContext : AbpDbContext<HttpApiHostMigrationsDbContext>
    {
        public HttpApiHostMigrationsDbContext(DbContextOptions<HttpApiHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureApiGateway();
        }
    }
}
