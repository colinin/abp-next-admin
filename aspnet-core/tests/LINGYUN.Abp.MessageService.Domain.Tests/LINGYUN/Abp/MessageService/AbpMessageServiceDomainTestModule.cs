using LINGYUN.Abp.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.MessageService
{
    [DependsOn(
        typeof(AbpMessageServiceDomainModule),
        typeof(AbpNotificationsTestsModule)
        )]
    public class AbpMessageServiceDomainTestModule : AbpModule
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
                    await dataSeeder.SeedAsync();
                    await scope.ServiceProvider
                        .GetRequiredService<AbpNotificationsTestDataBuilder>()
                        .BuildAsync();
                });
            }
        }
    }
}
