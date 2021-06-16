using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.EventBus.Abstractions;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.RealTime
{
    [DependsOn(typeof(AbpEventBusAbstractionsModule))]
    public class AbpRealTimeModule : AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            context
                .ServiceProvider
                .GetRequiredService<SnowflakeIdrGenerator>()
                .Initialize();
        }
    }
}
