using LY.MicroService.AuthServer.DbMigrator.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace LY.MicroService.AuthServer.DbMigrator;
public partial class AuthServerDbMigratorModule
{
    private void ConfigureDbContext(IServiceCollection services)
    {
        services.AddAbpDbContext<AuthServerMigrationsDbContext>();

        // 配置Ef
        Configure<AbpDbContextOptions>(options =>
        {
            options.UseMySQL();
        });
    }
}
