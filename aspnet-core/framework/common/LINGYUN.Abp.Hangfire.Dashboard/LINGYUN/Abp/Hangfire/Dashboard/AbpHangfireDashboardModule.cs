using Hangfire;
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
            var preActions = context.Services.GetPreConfigureActions<DashboardOptions>();
            context.Services.AddTransient(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<AbpHangfireDashboardOptionsProvider>().Get();
                preActions.Configure(options);

                return options;
            });
        }
    }
}
