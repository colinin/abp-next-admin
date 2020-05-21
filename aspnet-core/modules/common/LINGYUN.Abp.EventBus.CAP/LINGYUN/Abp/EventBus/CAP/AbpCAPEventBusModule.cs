using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.EventBus.CAP
{
    [DependsOn(typeof(AbpEventBusModule))]
    public class AbpCAPEventBusModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            context.Services.AddCAPEventBus(options =>
            {
                configuration.GetSection("CAP:EventBus").Bind(options);
                context.Services.ExecutePreConfiguredActions(options);
            });
        }
    }
}
