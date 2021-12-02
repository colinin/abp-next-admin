using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization;
using Volo.Abp.Hangfire;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Hangfire.Dashboard
{
    [DependsOn(
        typeof(AbpAuthorizationModule),
        typeof(AbpHangfireModule))]
    public class AbpHangfireDashboardModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<AbpHangfireDashboardOptionsProvider>().Get();
                return context.Services.ExecutePreConfiguredActions(options);
            });
        }
    }
}
