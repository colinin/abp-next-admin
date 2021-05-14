using LINGYUN.Abp.AspNetCore.HttpOverrides.Forwarded;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.AspNetCore.HttpOverrides
{
    public class AbpAspNetCoreHttpOverridesModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            var forwardOptions = new AbpForwardedHeadersOptions();
            configuration.GetSection("Forwarded:Headers").Bind(forwardOptions);
            context.Services.ExecutePreConfiguredActions(forwardOptions);

            Configure<ForwardedHeadersOptions>(options =>
            {
                options.Configure(forwardOptions);
            });
        }
    }
}
