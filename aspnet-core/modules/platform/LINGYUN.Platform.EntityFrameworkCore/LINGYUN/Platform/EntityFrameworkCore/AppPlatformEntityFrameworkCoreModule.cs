using LINGYUN.Platform.Routes;
using LINGYUN.Platform.Versions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Platform.EntityFrameworkCore
{
    [DependsOn(
        typeof(AppPlatformDomainModule),
        typeof(AbpEntityFrameworkCoreModule))]
    public class AppPlatformEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<PlatformDbContext>(options =>
            {
                options.AddRepository<Route, EfCoreRouteRepository>();
                options.AddRepository<AppVersion, EfCoreVersionRepository>();

                options.AddDefaultRepositories(includeAllEntities: true);

                options.Entity<AppVersion>(appVersion =>
                {
                    appVersion.DefaultWithDetailsFunc = (x) =>
                        x.Include(q => q.Files);
                });
            });
        }
    }
}
