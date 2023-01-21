using LY.MicroService.Platform.DbMigrator.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace LY.MicroService.Platform.DbMigrator;
public partial class PlatformDbMigratorModule
{
    private void ConfigureDbContext(IServiceCollection services)
    {
        services.AddAbpDbContext<PlatformMigrationsDbContext>();

        // 配置Ef
        Configure<AbpDbContextOptions>(options =>
        {
            options.UseMySQL();
        });
    }
}
