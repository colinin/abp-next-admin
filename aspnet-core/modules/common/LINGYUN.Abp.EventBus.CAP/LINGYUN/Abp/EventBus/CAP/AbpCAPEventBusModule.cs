using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.EventBus.CAP
{
    /// <summary>
    /// AbpCAPEventBusModule
    /// </summary>
    [DependsOn(typeof(AbpEventBusModule))]
    public class AbpCAPEventBusModule : AbpModule
    {
        /// <summary>
        /// ConfigureServices
        /// </summary>
        /// <param name="context"></param>
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
