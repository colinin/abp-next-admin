using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;

namespace LY.MicroService.IdentityServer.EntityFrameworkCore;

public class IdentityServertMigrationsDbContext : AbpDbContext<IdentityServertMigrationsDbContext>
{
    public IdentityServertMigrationsDbContext(DbContextOptions<IdentityServertMigrationsDbContext> options)
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
