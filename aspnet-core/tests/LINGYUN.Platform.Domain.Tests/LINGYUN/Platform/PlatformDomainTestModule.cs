using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace LINGYUN.Platform
{
    [DependsOn(
        typeof(PlatformDomainModule)
        )]
    public class PlatformDomainTestModule : AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            SeedTestData(context);
        }

        private static void SeedTestData(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                var dataSeeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
                AsyncHelper.RunSync(async () =>
                {
                    // 预置宿主数据
                    await dataSeeder.SeedAsync();
                    // 预置租户数据
                    await dataSeeder.SeedAsync(PlatformTestsConsts.TenantId);
                });
                AsyncHelper.RunSync(async () =>
                {
                    await scope.ServiceProvider
                        .GetRequiredService<PlatformTestsDataBuilder>()
                        .BuildAsync();
                });
            }
        }
    }
}
