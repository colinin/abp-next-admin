using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.AspNetCore.SignalR.JwtToken
{
    [DependsOn(
        typeof(AbpAspNetCoreSignalRModule))]
    public class AbpAspNetCoreSignalRJwtTokenModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<AbpAspNetCoreSignalRJwtTokenMapPathOptions>(configuration.GetSection("SignalR:MapPath"));
        }
    }
}
