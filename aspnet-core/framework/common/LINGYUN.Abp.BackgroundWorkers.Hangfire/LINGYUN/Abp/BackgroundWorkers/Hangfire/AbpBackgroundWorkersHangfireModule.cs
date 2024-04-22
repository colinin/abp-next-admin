using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Hangfire;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BackgroundWorkers.Hangfire
{
    [DependsOn(
        typeof(AbpBackgroundWorkersModule),
        typeof(AbpHangfireModule)
    )]
    public class AbpBackgroundWorkersHangfireModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton(typeof(HangfireBackgroundWorkerAdapter<>));
        }
    }
}
