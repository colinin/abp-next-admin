using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;

namespace AuthServer.EntityFrameworkCore
{
    public class AuthServerHostMigrationsDbContext : AbpDbContext<AuthServerHostMigrationsDbContext>
    {
        public AuthServerHostMigrationsDbContext(DbContextOptions<AuthServerHostMigrationsDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureIdentity();
            modelBuilder.ConfigureIdentityServer();
        }
    }
}
