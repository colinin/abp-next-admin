using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Identity.EntityFrameworkCore
{
    [DependsOn(typeof(LINGYUN.Abp.Identity.AbpIdentityDomainModule))]
    [DependsOn(typeof(Volo.Abp.Identity.EntityFrameworkCore.AbpIdentityEntityFrameworkCoreModule))]
    public class AbpIdentityEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<IdentityDbContext>(options =>
            {
                options.AddRepository<IdentityRole, EfCoreIdentityRoleRepository>();
            });
        }
    }
}
