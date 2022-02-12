using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Logging
{
    public class AbpLoggingModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<AbpLoggingOptions>(configuration.GetSection("Logging"));
        }
    }
}
