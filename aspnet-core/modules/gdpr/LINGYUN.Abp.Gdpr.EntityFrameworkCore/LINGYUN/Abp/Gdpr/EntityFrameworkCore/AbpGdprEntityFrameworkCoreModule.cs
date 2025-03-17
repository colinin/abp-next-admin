using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Gdpr.EntityFrameworkCore;

[DependsOn(
    typeof(AbpGdprDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
    )]
public class AbpGdprEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<GdprDbContext>(options =>
        {
            options.AddRepository<GdprRequest, EfCoreGdprRequestRepository>();

            options.AddDefaultRepositories<IGdprDbContext>();
        });
    }
}
