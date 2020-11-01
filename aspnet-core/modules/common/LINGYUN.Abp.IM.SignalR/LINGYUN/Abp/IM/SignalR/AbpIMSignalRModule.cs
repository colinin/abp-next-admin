using LINGYUN.Abp.AspNetCore.SignalR.JwtToken;
using LINGYUN.Abp.IM.SignalR.Messages;
using LINGYUN.Abp.RealTime.SignalR;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.IM.SignalR
{
    [DependsOn(
        typeof(AbpIMModule),
        typeof(AbpRealTimeSignalRModule),
        typeof(AbpAspNetCoreSignalRJwtTokenModule))]
    public class AbpIMSignalRModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpIMOptions>(options =>
            {
                options.Providers.Add<SignalRMessageSenderProvider>();
            });

            Configure<AbpAspNetCoreSignalRJwtTokenMapPathOptions>(options =>
            {
                options.MapPath("messages");
            });
        }
    }
}
