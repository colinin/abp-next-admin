using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;

namespace LY.MicroService.IdentityServer.EntityFrameworkCore;

[ConnectionStringName("IdentityServerDbMigrator")]
public class IdentityServerMigrationsDbContext : AbpDbContext<IdentityServerMigrationsDbContext>
{
    public IdentityServerMigrationsDbContext(DbContextOptions<IdentityServerMigrationsDbContext> options)
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
