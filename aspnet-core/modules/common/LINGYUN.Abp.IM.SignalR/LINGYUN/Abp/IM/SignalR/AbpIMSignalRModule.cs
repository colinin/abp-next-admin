using LINGYUN.Abp.AspNetCore.SignalR.JwtToken;
using LINGYUN.Abp.RealTime;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.IM.SignalR
{
    [DependsOn(
        typeof(AbpRealTimeModule),
        typeof(AbpAspNetCoreSignalRModule),
        typeof(AbpAspNetCoreSignalRJwtTokenModule))]
    public class AbpIMSignalRModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAspNetCoreSignalRJwtTokenMapPathOptions>(options =>
            {
                options.MapPath("messages");
            });
        }
    }
}
