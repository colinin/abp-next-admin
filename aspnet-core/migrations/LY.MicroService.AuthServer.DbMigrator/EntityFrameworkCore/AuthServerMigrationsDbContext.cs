using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;

namespace LY.MicroService.AuthServer.DbMigrator.EntityFrameworkCore;

[ConnectionStringName("AuthServerDbMigrator")]
public class AuthServerMigrationsDbContext : AbpDbContext<AuthServerMigrationsDbContext>
{
    public AuthServerMigrationsDbContext(DbContextOptions<AuthServerMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureIdentity();
        modelBuilder.ConfigureOpenIddict();
    }
}
