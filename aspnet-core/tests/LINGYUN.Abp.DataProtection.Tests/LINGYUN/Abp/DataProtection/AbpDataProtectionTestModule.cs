using LINGYUN.Abp.DataProtection.EntityFrameworkCore;
using LINGYUN.Abp.EntityFrameworkCore.Tests;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.DataProtection
{
    [DependsOn(
        typeof(AbpDataProtectionEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreTestModule))]
    public class AbpDataProtectionTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<FakeDataProtectedDbContext>(options =>
            {
                options.AddRepository<FakeProtectionObject, EfCoreFakeProtectionObjectRepository>();
            });
        }
    }
}
