using LY.MicroService.LocalizationManagement.DbMigrator.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace LY.MicroService.LocalizationManagement.DbMigrator;
public partial class LocalizationManagementDbMigratorModule
{
    private void ConfigureDbContext(IServiceCollection services)
    {
        services.AddAbpDbContext<LocalizationManagementMigrationsDbContext>();

        // 配置Ef
        Configure<AbpDbContextOptions>(options =>
        {
            options.UseMySQL();
        });
    }
}
