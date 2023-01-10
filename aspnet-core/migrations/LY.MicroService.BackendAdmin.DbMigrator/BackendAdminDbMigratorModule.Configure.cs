using LY.MicroService.BackendAdmin.DbMigrator.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace LY.MicroService.BackendAdmin.DbMigrator;
public partial class BackendAdminDbMigratorModule
{
    private void ConfigureDbContext(IServiceCollection services)
    {
        services.AddAbpDbContext<BackendAdminMigrationsDbContext>();

        // 配置Ef
        Configure<AbpDbContextOptions>(options =>
        {
            options.UseMySQL();
        });
    }
}
